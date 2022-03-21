using System;
using System.Collections.Generic;
using System.Linq;
using EarthIsMine.UI;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        [field: SerializeField]
        public Canvas SceneCanvas { get; private set; }

        [SerializeField]
        private Presenter[] _showInStart;

        public IReadOnlyDictionary<Type, Presenter> Presenters => _presenters;

        private Dictionary<Type, Presenter> _presenters;

        protected override void Awake()
        {
            base.Awake();

            var presenters = SceneCanvas.gameObject.GetComponentsInChildren<Presenter>(true);
            _presenters = presenters.ToDictionary(presenter => presenter.GetType());
        }

        private void Start()
        {
            if (_showInStart is not null)
            {
                foreach (var presenter in _showInStart)
                {
                    presenter.gameObject.SetActive(true);
                }
            }
        }

        public T GetPresenter<T>() where T : Presenter
        {
            if (_presenters.TryGetValue(typeof(T), out var presenter))
            {
                return presenter as T;
            }
            return null;
        }

        public void Show<T>(bool closeOthers = true) where T : Presenter
        {
            if (!_presenters.TryGetValue(typeof(T), out var presenter))
            {
                Debug.LogWarning($"Presenter Type '{typeof(T)}' is not in UIManager.");
                return;
            }

            presenter.gameObject.SetActive(true);
            if (closeOthers)
            {
                var targets = _presenters.Values.Where(p => !p.IsDefault && p != presenter);
                foreach (var target in targets)
                {
                    target.gameObject.SetActive(false);
                }
            }
        }
    }
}
