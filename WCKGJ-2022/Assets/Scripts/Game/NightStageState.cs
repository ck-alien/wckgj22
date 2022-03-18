using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;
using UniRx;

namespace EarthIsMine.Game
{
    public class NightStageState : StateBehaviour
    {
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
            var wait = UniTask.Delay(500).ToObservable().ToYieldInstruction();
            while (!wait.IsDone)
            {
                yield return null;
            }
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
