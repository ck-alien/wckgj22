using System.Collections;
using UnityEngine;

namespace EarthIsMine.FSM
{
    public interface IStateBehaviour
    {
        IEnumerator OnEnter(IStateMachine stateMachine);
        IEnumerator OnExecute(IStateMachine stateMachine);
        IEnumerator OnExit(IStateMachine stateMachine);
    }

    public abstract class StateBehaviour : MonoBehaviour, IStateBehaviour
    {
        public abstract IEnumerator OnEnter(IStateMachine stateMachine);
        public abstract IEnumerator OnExecute(IStateMachine stateMachine);
        public abstract IEnumerator OnExit(IStateMachine stateMachine);
    }
}
