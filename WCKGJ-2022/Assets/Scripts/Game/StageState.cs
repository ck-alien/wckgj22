using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EarthIsMine.Data;
using EarthIsMine.FSM;
using EarthIsMine.Manager;
using EarthIsMine.Object;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EarthIsMine.Game
{
    [Serializable]
    public class StageInfo
    {
        [field: SerializeField]
        public StageTypes Type { get; private set; }

        [field: SerializeField]
        public StageData StageData { get; private set; }

        [field: SerializeField]
        public FMODUnity.EventReference BGM { get; private set; }

        public FMOD.Studio.EventInstance BGMInstance { get; set; }
    }

    public class StageState : StateBehaviour
    {
        [SerializeField]
        private StageInfo[] _waves;

        [SerializeField]
        private BackGroundController[] _backgrounds;

        [SerializeField]
        private Light2D _light;

        [SerializeField]
        private UnityEngine.Rendering.Volume _nightVolume;

        private StageInfo _stage;
        private IDisposable _pauseCheck;

        private void Start()
        {
            foreach (var wave in _waves)
            {
                if (!wave.BGM.IsNull)
                {
                    wave.BGMInstance = FMODUnity.RuntimeManager.CreateInstance(wave.BGM);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var wave in _waves)
            {
                if (wave.BGMInstance.isValid())
                {
                    wave.BGMInstance.release();
                    wave.BGMInstance.clearHandle();
                }
            }
        }

        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            var gameStateMachine = stateMachine as GameStateMachine;
            _stage = Array.Find(_waves, w => w.Type == gameStateMachine.StageType);
            Debug.Assert(_stage is not null, $"{gameStateMachine.StageType}에 맞는 웨이브 데이터가 없습니다.");

            Debug.Log($"{gameStateMachine.StageType} Stage Start");

            switch (gameStateMachine.StageType)
            {
                case StageTypes.Day:
                    DOTween.To(() => _nightVolume.weight, w => _nightVolume.weight = w, 0f, 2f);
                    DOTween.To(() => _light.color, c => _light.color = c, Color.white, 2f);
                    break;
                case StageTypes.Night:
                    DOTween.To(() => _nightVolume.weight, w => _nightVolume.weight = w, 1f, 2f);
                    DOTween.To(() => _light.color, c => _light.color = c, Color.gray, 2f);
                    break;
                default:
                    break;
            }

            if (gameStateMachine.StageType == StageTypes.Night)
            {
                //for (int i = 0; i < _backgrounds.Length; i++)
                //{
                //    _backgrounds[i].ChangeSprite(Day.Night);
                //}
            }
            else
            {
                //for (int i = 0; i < _backgrounds.Length; i++)
                //{
                //    _backgrounds[i].ChangeSprite(Day.DayTime);
                //}
            }

            if (_stage.BGMInstance.isValid())
            {
                _stage.BGMInstance.start();
            }

            if (_pauseCheck is not null)
            {
                _pauseCheck.Dispose();
            }
            _pauseCheck = GameManager.Instance.IsPaused.Subscribe(paused => _stage.BGMInstance.setPaused(paused));

            var wait = UniTask.Delay(1000).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation)
        {
            var gameStateMachine = stateMachine as GameStateMachine;

            var waves = _stage.StageData.Waves;

            for (int i = 0; i < waves.Length; i++)
            {
                var wave = waves[i];
                Debug.Log($"Start Wave [{i}]");

                SpawnEnemies(wave.SpawnPattern);

                do
                {
                    yield return null;
                    if (cancellation.IsCancellationRequested)
                    {
                        yield break;
                    }
                } while (EnemyManager.Instance.ActiveObjectsCount > 0);
            }

            gameStateMachine.StageType = gameStateMachine.StageType switch
            {
                StageTypes.Day => StageTypes.Night,
                StageTypes.Night => StageTypes.Day,
                _ => StageTypes.Day
            };

            void SpawnEnemies(WavePattern[] patterns)
            {
                var spawnPosition = gameStateMachine.EnemySpawnPosition;
                var distance = gameStateMachine.EnemySpawnDistance;

                foreach (var spawnInfo in patterns)
                {
                    var pos = spawnPosition;
                    pos.x += distance.x * spawnInfo.RelativePosition.x;
                    pos.y += distance.y * spawnInfo.RelativePosition.y;

                    EnemyManager.Instance.Spawn(spawnInfo.EnemyType, pos);
                }
            }
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            if (_stage.BGMInstance.isValid())
            {
                _stage.BGMInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            if (_pauseCheck is not null)
            {
                _pauseCheck.Dispose();
                _pauseCheck = null;
            }

            yield break;
        }
    }
}

