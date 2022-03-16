using System.Collections;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;

namespace EarthIsMine.Game
{
    public class ReadyBehaviour : StateBehaviour
    {
        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(500).ToCoroutine();
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(500).ToCoroutine();

            stateMachine.ChangeState(typeof(StageBehaviour));
            yield break;
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(500).ToCoroutine();
        }
    }
}
