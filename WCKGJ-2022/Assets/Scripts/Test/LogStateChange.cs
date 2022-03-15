using EarthIsMine.FSM;
using UnityEngine;

public class LogStateChange : MonoBehaviour
{
    public void HandleStateChanged(IStateMachine stateMachine)
    {
        Debug.Log($"StateChanged: {stateMachine.CurrentState.GetType().Name}");
    }

    public void HandleBehaviourStateUpdated(IStateMachine stateMachine)
    {
        Debug.Log($"BehaviourStateUpdated: {stateMachine.CurrentBehaviourState}");
    }
}
