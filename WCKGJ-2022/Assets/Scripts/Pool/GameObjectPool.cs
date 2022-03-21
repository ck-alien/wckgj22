using System;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Pool
{
    public interface IReadonlyGameObjectPool
    {
        IReadOnlyCollection<GameObject> ActiveObjects { get; }
        IReadOnlyCollection<GameObject> InactiveObjects { get; }

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
        public IReadOnlyCollection<GameObject> ActiveObjects => _activeObjects;
        public IReadOnlyCollection<GameObject> InactiveObjects => _inactiveObjects;

        public int CountAll => _activeObjects.Count + _inactiveObjects.Count;
        public int CountActive => _activeObjects.Count;
        public int CountInactive => _inactiveObjects.Count;

        private readonly LinkedList<GameObject> _activeObjects;
        private readonly Queue<GameObject> _inactiveObjects;
        private readonly GameObject _prefab;
        private readonly Dictionary<Guid, LinkedListNode<GameObject>> _cache;

        public GameObjectPool(GameObject prefab, int capacity = 50)
        {
            _prefab = prefab;
            _activeObjects = new LinkedList<GameObject>();
            _inactiveObjects = new Queue<GameObject>(capacity);
            _cache = new Dictionary<Guid, LinkedListNode<GameObject>>(capacity);
        }

        public GameObject Take()
        {
            if (!_inactiveObjects.TryDequeue(out var item))
            {
                item = UnityEngine.Object.Instantiate(_prefab);
            }
            if (!item.TryGetComponent<PoolingItemGUID>(out var component))
            {
                component = item.AddComponent<PoolingItemGUID>();
            }
            var guid = component.GUID;

            var node = _activeObjects.AddLast(item);
            if (!_cache.TryAdd(guid, node))
            {
                _activeObjects.Remove(node);
                throw new InvalidOperationException("풀링오브젝트 캐시 추가에 실패했습니다.");
            }

            if (!item.TryGetComponent<ReturnToPool>(out var returnToPool))
            {
                returnToPool = item.AddComponent<ReturnToPool>();
            }
            returnToPool.Pool = this;

            item.SetActive(true);
            return node.Value;
        }

        public void Release(GameObject item)
        {
            var guid = item.GetComponent<PoolingItemGUID>().GUID;
            if (!_cache.Remove(guid, out var node))
            {
                Debug.LogWarning($"{guid}와 일치하는 노드를 찾지 못했습니다.");
                // throw new InvalidOperationException($"{guid}와 일치하는 노드를 찾지 못했습니다.");
            }

            item.SetActive(false);
            _activeObjects.Remove(node);
            _inactiveObjects.Enqueue(item);
        }

        public void Clear()
        {
            while (_inactiveObjects.TryDequeue(out var item))
            {
                UnityEngine.Object.Destroy(item);
            }
            foreach (var item in _activeObjects)
            {
                UnityEngine.Object.Destroy(item);
            }
            _activeObjects.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
