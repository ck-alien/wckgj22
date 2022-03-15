using System.Collections;
using Cysharp.Threading.Tasks;
using EarthIsMine.FSM;

namespace EarthIsMine.Game
{
    public class NightStageBehaviour : StateBehaviour
    {
        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(1000).ToCoroutine();
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(3000).ToCoroutine();

            stateMachine.ChangeState(typeof(ReadyBehaviour));
            yield break;
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            yield return UniTask.Delay(1000).ToCoroutine();
        }
    }
}
