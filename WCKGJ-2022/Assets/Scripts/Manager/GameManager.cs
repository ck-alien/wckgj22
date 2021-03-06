using System;
using System.Collections;
using EarthIsMine.Data;
using EarthIsMine.Game;
using EarthIsMine.Object;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EarthIsMine.Manager
{
    public enum StageType
    {
        Day, Night
    }

    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField]
        public GameData Data { get; private set; }

        [field: SerializeField]
        public GameStateMachine Game { get; private set; }

        [field: SerializeField]
        public Player Player { get; private set; }

        public ReactiveProperty<bool> IsPaused { get; set; } = new();
        public ReactiveProperty<bool> IsGameOver { get; private set; } = new();

        public ReactiveProperty<int> Score { get; set; } = new();

        public ReactiveProperty<StageType> CurrentStage { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            IsPaused.Where(isPaused => isPaused is true).Subscribe(_ => Time.timeScale = 0f);
            IsPaused.Where(isPaused => isPaused is false).Subscribe(_ => Time.timeScale = 1f);
        }

        private void Start()
        {
            Observable
                .FromMicroCoroutine<int>((observer) => AutoScoring(observer))
                .Subscribe(addScore => Score.Value += addScore)
                .AddTo(gameObject);

            Player.Life.Where(life => life < 1).Subscribe(_ =>
            {
                print("Game Over");
                IsGameOver.Value = true;
                Game.ChangeState(typeof(DeadState));
            });
        }

        private void OnEnable()
        {
            if (GameInput.Instance)
            {
                GameInput.Instance.ActionMaps.Game.Pause.performed += OnPause;
            }
        }

        private void OnDisable()
        {
            if (GameInput.Instance)
            {
                GameInput.Instance.ActionMaps.Game.Pause.performed -= OnPause;
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsPaused.Value = !IsPaused.Value;
            }
        }

        private IEnumerator AutoScoring(IObserver<int> observer)
        {
            float time = 0f;

            while (true)
            {
                if (IsGameOver.Value)
                {
                    yield break;
                }

                time += Time.deltaTime;
                if (time >= Data.TimeScoreInterval)
                {
                    observer.OnNext(Data.TimeScore);
                    time = 0f;
                }
                yield return null;
            }
        }
    }
}
