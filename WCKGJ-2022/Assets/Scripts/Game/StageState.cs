using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;
using EarthIsMine.Manager;
using EarthIsMine.Object;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Game
{
    public class StageState : StateBehaviour
    {
        private int _idx = 0;

        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            var wait = UniTask.Delay(500).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation)
        {
            switch (_idx)
            {
                case 0:
                    EnemyManager.Instance.Spawn<EnemyTypeA>(new Vector3(0f, 3f, 0f));
                    break;

                case 1:
                    EnemyManager.Instance.Spawn<EnemyTypeB>(new Vector3(0f, 3f, 0f));
                    break;

                case 2:
                    EnemyManager.Instance.Spawn<EnemyTypeC>(new Vector3(0f, 3f, 0f));
                    break;

                default:
                    break;
            }
            _idx = _idx < 2 ? _idx + 1 : 0;

            var time = 0f;
            while (!cancellation.IsCancellationRequested && time <= 1f)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (cancellation.IsCancellationRequested)
            {
                yield break;
            }

            stateMachine.ChangeState(typeof(ReadyState));
            yield break;
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            var wait = UniTask.Delay(500).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
        }
    }
}

