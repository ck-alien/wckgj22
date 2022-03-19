using System;
using System.Collections;
using EarthIsMine.Player;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace EarthIsMine.Manager
{
    public enum StageType
    {
        Day, Night
    }

    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField]
        public PlayerController Player { get; private set; }

        public ReactiveProperty<bool> IsPaused { get; set; } = new();
        public ReactiveProperty<bool> IsGameOver { get; private set; } = new();

        public ReactiveProperty<uint> Score { get; set; } = new();

        public ReactiveProperty<StageType> CurrentStage { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SceneManager.LoadSceneAsync("BackgroundScene", LoadSceneMode.Additive);

            IsPaused.Where(isPaused => isPaused is true).Subscribe(_ => Time.timeScale = 0f);
            IsPaused.Where(isPaused => isPaused is false).Subscribe(_ => Time.timeScale = 1f);
        }

        private void Start()
        {
            Observable
                .FromMicroCoroutine<uint>((observer) => AutoScoring(observer))
                .Subscribe(addScore => Score.Value += addScore)
                .AddTo(gameObject);

            Observable
                .FromCoroutine<StageType>((observer) => Run(observer));
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsPaused.Value = !IsPaused.Value;
            }
        }

        private IEnumerator AutoScoring(IObserver<uint> observer)
        {
            float deltaTime = 0f;

            while (true)
            {
                deltaTime += Time.deltaTime;
                if (deltaTime >= 10.0f)
                {
                    var integerPart = Mathf.Floor(deltaTime);
                    deltaTime -= integerPart;
                    observer.OnNext(50);
                }
                yield return null;
            }
        }

        private IEnumerator Run(IObserver<StageType> observer)
        {
            yield break;
        }
    }
}
