using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EarthIsMine.FSM
{
    public interface IStateMachine
    {
        StateBehaviour[] States { get; }
        StateBehaviour CurrentState { get; }
        BehaviourStates CurrentBehaviourState { get; }

        bool ChangeState(Type nextStateType);
    }

    public abstract class StateMachine : MonoBehaviour, IStateMachine
    {
        [field: SerializeField]
        public StateBehaviour[] States { get; private set; }

        [field: SerializeField]
        public UnityEvent<IStateMachine> OnStateChanged { get; private set; }

        [field: SerializeField]
        public UnityEvent<IStateMachine> OnBehaviourStateUpdated { get; private set; }

        // ONLY DEBUG
#pragma warning disable IDE0052 // Remove unread private members
        [Header("[DEBUG | ReadOnly]")]
        [SerializeField] private string _currentStateName;
        [SerializeField] private string _currentBehaviourStateName;
#pragma warning restore IDE0052

        public StateBehaviour CurrentState { get; private set; }
        public BehaviourStates CurrentBehaviourState { get; private set; } = BehaviourStates.Enter;

        private StateBehaviour _nextScheduledState;

        protected virtual void Awake()
        {
            if (States.Length > 0)
            {
                _nextScheduledState = States[0];
            }
        }

        protected virtual void Start()
        {
            if (_nextScheduledState is null)
            {
                gameObject.SetActive(false);
            }

            _currentStateName = _nextScheduledState.GetType().Name;
            _currentBehaviourStateName = CurrentBehaviourState.ToString();
            StartCoroutine(Run());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator Run()
        {
            while (true)
            {
                if (_nextScheduledState is not null)
                {
                    ChangeState(_nextScheduledState);
                    _nextScheduledState = null;
                }

                var state = CurrentState;

                ChangeBehaviourState(BehaviourStates.Enter);
                yield return StartCoroutine(state.OnEnter(this));

                ChangeBehaviourState(BehaviourStates.Execute);
                yield return StartCoroutine(state.OnExecute(this));

                ChangeBehaviourState(BehaviourStates.Exit);
                yield return StartCoroutine(state.OnExit(this));
            }

            void ChangeState(StateBehaviour state)
            {
                CurrentState = state;
                _currentStateName = CurrentState.GetType().Name;
                OnStateChanged?.Invoke(this);
            }

            void ChangeBehaviourState(BehaviourStates state)
            {
                CurrentBehaviourState = state;
                _currentBehaviourStateName = CurrentBehaviourState.ToString();
                OnBehaviourStateUpdated?.Invoke(this);
            }
        }

        public bool ChangeState(Type nextStateType)
        {
            if (CurrentBehaviourState is BehaviourStates.Enter or BehaviourStates.Exit)
            {
                Debug.LogWarning("ChangeState cannot process in Enter and Exit.");
                return false;
            }

            if (_nextScheduledState is not null)
            {
                Debug.LogWarning("Already scheduled next state.");
                return false;
            }

            var nextState = Array.Find(States, s => s.GetType() == nextStateType);
            if (nextState is null)
            {
                Debug.LogWarning($"Cannot found state type of {nextStateType.Name}");
                return false;
            }

            _nextScheduledState = nextState;
            return true;
        }
    }
}
