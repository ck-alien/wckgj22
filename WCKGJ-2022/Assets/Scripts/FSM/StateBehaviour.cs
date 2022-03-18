using System.Collections;
using System.Threading;
using UnityEngine;

namespace EarthIsMine.FSM
{
    public interface IStateBehaviour
    {
        IEnumerator OnEnter(IStateMachine stateMachine);
        IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation);
        IEnumerator OnExit(IStateMachine stateMachine);
    }

    public abstract class StateBehaviour : MonoBehaviour, IStateBehaviour
    {
        public abstract IEnumerator OnEnter(IStateMachine stateMachine);
        public abstract IEnumerator OnExecute(IStateMachine stateMachine, CancellationToken cancellation);
        public abstract IEnumerator OnExit(IStateMachine stateMachine);
    }
}
