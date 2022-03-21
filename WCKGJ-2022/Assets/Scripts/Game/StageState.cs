using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using EarthIsMine.Data;
using EarthIsMine.FSM;
using EarthIsMine.Manager;
using EarthIsMine.Object;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Game
{
    [Serializable]
    public class StageInfo
    {
        [field: SerializeField]
        public StageTypes Type { get; private set; }

        [field: SerializeField]
        public StageWaveData StageData { get; private set; }

        [field: SerializeField]
        public FMODUnity.EventReference BGM { get; private set; }

        public FMOD.Studio.EventInstance BGMInstance { get; set; }
    }

    public class StageState : StateBehaviour
    {
        [SerializeField]
        private StageInfo[] _waves;

        private StageInfo _stage;

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
            var controllers = FindObjectsOfType<BackGroundController>();
            var gameStateMachine = stateMachine as GameStateMachine;
            _stage = Array.Find(_waves, w => w.Type == gameStateMachine.StageType);
            Debug.Assert(_stage is not null, $"{gameStateMachine.StageType}에 맞는 웨이브 데이터가 없습니다.");

            Debug.Log($"{gameStateMachine.StageType} Stage Start");
            if (gameStateMachine.StageType == StageTypes.Night)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    controllers[i].ChangeSprite(Day.Night);
                }
            }
            else
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    controllers[i].ChangeSprite(Day.DayTime);
                }
            }



            if (_stage.BGMInstance.isValid())
            {
                _stage.BGMInstance.start();
            }

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
            foreach (var wave in waves)
            {
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

            void SpawnEnemies(EnemyPatternDataItem[] patterns)
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
            yield break;
        }
    }
}

