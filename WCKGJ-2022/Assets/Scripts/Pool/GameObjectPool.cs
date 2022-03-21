using System;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Pool
{
    public interface IReadonlyGameObjectPool
    {
        int CountAll { get; }
        int CountActive { get; }
        int CountInactive { get; }
    }

    public interface IGameObjectPool : IReadonlyGameObjectPool
    {
        GameObject Take();
        void Release(GameObject item);
        void Clear();
    }

    public class GameObjectPool : IGameObjectPool, IDisposable
    {
        public int CountAll => _items.Count;
        public int CountActive => _items.Count - CountInactive;
        public int CountInactive { get; private set; }

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
                    CountInactive--;
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
            foreach (var check in _items)
            {
                if (item == check)
                {
                    item.SetActive(false);
                    CountInactive++;
                    return;
                }
            }
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
