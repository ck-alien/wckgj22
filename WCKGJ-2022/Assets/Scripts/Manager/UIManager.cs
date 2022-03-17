using System;
using System.Collections.Generic;
using System.Linq;
using EarthIsMine.UI;
using UnityEngine;

namespace EarthIsMine.Manager
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private Presenter[] _presenterPrefabs;

        public Presenter Presenter { get; private set; } = null;

        private Dictionary<Type, Presenter> _presenters;

        private void Start()
        {
            _presenters = _presenterPrefabs
                .Select(prefab =>
                {
                    var obj = Instantiate(prefab.gameObject, transform);
                    obj.SetActive(false);
                    return obj.GetComponent<Presenter>();
                })
                .ToDictionary(presenter => presenter.GetType());
        }

        public T GetPresenter<T>() where T : Presenter
        {
            if (_presenters.TryGetValue(typeof(T), out var presenter))
            {
                return presenter as T;
            }
            return null;
        }

        public void Show<T>() where T : Presenter
        {
            if (Presenter is not null)
            {
                Presenter.gameObject.SetActive(false);
            }

            if (!_presenters.TryGetValue(typeof(T), out var presenter))
            {
                Debug.LogWarning($"Presenter Type '{typeof(T)}' is not in UIManager.");
                return;
            }

            presenter.gameObject.SetActive(true);
            Presenter = presenter;
        }
    }
}
