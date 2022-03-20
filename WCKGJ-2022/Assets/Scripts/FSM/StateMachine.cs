using System;
using System.Collections;
using System.Threading;
using UniRx;
using UnityEngine;

namespace EarthIsMine.FSM
{
    public interface IStateMachine
    {
        StateBehaviour[] States { get; }
        ReactiveProperty<StateBehaviour> CurrentState { get; }

        IStateBehaviour GetState(Type stateType);
        bool ChangeState(Type nextStateType);
    }

    public abstract class StateMachine : MonoBehaviour, IStateMachine
    {
        [field: SerializeField]
        public StateBehaviour[] States { get; private set; }

        // ONLY DEBUG
#pragma warning disable IDE0052 // Remove unread private members
        [Header("[DEBUG | ReadOnly]")]
        [SerializeField] private string _currentStateName;
#pragma warning restore IDE0052

        public ReactiveProperty<StateBehaviour> CurrentState { get; private set; } = new();

        private StateBehaviour _nextScheduledState;
        private CancellationTokenSource _stateCancellation = new();

        protected virtual void Awake()
        {
            if (States.Length > 0)
            {
                _nextScheduledState = States[0];
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void Start()
        {
            CurrentState
                .Where(state => state is not null)
                .Subscribe(state => _currentStateName = state.GetType().Name);

            Observable
                .FromMicroCoroutine<StateBehaviour>((observer) => Run(observer))
                .Subscribe(state => CurrentState.Value = state)
                .AddTo(gameObject);
        }

        protected virtual void OnDestroy()
        {
            _stateCancellation?.Cancel();
        }

        private IEnumerator Run(IObserver<StateBehaviour> observer)
        {
            while (true)
            {
                if (_stateCancellation.IsCancellationRequested)
                {
                    _stateCancellation.Dispose();
                    _stateCancellation = new CancellationTokenSource();
                }

                observer.OnNext(_nextScheduledState);
                var state = _nextScheduledState;
                _nextScheduledState = null;

                var stateRunner = Observable.FromMicroCoroutine(() => state.OnEnter(this))
                    .SelectMany(() => state.OnExecute(this, _stateCancellation.Token))
                    .SelectMany(() => state.OnExit(this))
                    .ToYieldInstruction();

                while (!stateRunner.IsDone)
                {
                    yield return null;
                }

                _nextScheduledState = _nextScheduledState is null ? state : _nextScheduledState;
                yield return null;
            }
        }

        public IStateBehaviour GetState(Type stateType)
        {
            return Array.Find(States, s => s.GetType() == stateType);
        }

        public bool ChangeState(Type nextStateType)
        {
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
            _stateCancellation.Cancel();
            return true;
        }
    }
}
