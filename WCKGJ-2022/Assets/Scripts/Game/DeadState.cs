using System.Collections;
using System.Threading;
using EarthIsMine.FSM;
using UnityEngine;

namespace EarthIsMine.Game
{
    public class DeadState : StateBehaviour
    {
        public override IEnumerator OnEnter(IStateMachine stateMachine)
        {
            Debug.Log("Enter Dead State");
            yield break;
        }

        public override IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                yield return null;
            }
        }

        public override IEnumerator OnExit(IStateMachine stateMachine)
        {
            yield break;
        }
    }
}
