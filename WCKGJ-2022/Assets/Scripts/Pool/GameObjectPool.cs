using System;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Pool
{
    public interface IReadonlyGameObjectPool
    {
        IReadOnlyCollection<GameObject> PoolItems { get; }
    }

    public interface IGameObjectPool : IReadonlyGameObjectPool
    {
        GameObject Take();
        void Release(GameObject item);
        void Clear();
    }

    public class GameObjectPool : IGameObjectPool, IDisposable
    {
        public IReadOnlyCollection<GameObject> PoolItems => _items;

        private readonly GameObject _prefab;
        private readonly List<GameObject> _items;

        public GameObjectPool(GameObject prefab, int capacity = 50)
        {
            _prefab = prefab;
            _items = new List<GameObject>(capacity);
        }

        public GameObject Take()
        {
            foreach (var item in _items)
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }

            var newItem = UnityEngine.Object.Instantiate(_prefab);
            if (!newItem.TryGetComponent<ReturnToPool>(out var returnToPool))
            {
                returnToPool = newItem.AddComponent<ReturnToPool>();
            }
            returnToPool.Pool = this;

            _items.Add(newItem);
            return newItem;
        }

        public void Release(GameObject item)
        {
            item.SetActive(false);
            return;
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                UnityEngine.Object.Destroy(item);
            }
            _items.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
