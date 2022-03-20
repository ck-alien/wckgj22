using System;
using System.Collections.Generic;
using EarthIsMine.Object;
using EarthIsMine.Pool;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public abstract class ObjectManager<ManagerT, ItemT> : Singleton<ManagerT>
        where ManagerT : MonoBehaviour
        where ItemT : MonoBehaviour, IJobObject
    {
        [SerializeField, Tooltip("오브젝트 프리팹 리스트")]
        private ItemT[] _prefabs;

        /// <summary>
        /// 활성화된 오브젝트들
        /// </summary>
        public IReadOnlyList<ItemT> ActiveObjects => _activeObjects;

        /// <summary>
        /// 프리팹 타입 별 오브젝트풀
        /// </summary>
        protected IReadOnlyDictionary<Type, IGameObjectPool> Pool => _pool;

        private readonly Dictionary<Type, GameObject> _poolingPrefabsCache = new();
        private readonly Dictionary<Type, IGameObjectPool> _pool = new();
        private readonly List<ItemT> _activeObjects = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in _prefabs)
            {
                var objectType = item.GetType();
                var pool = new GameObjectPool(item.gameObject);

                _poolingPrefabsCache.TryAdd(objectType, item.gameObject);
                _pool.TryAdd(objectType, pool);
            }
        }

        protected T GetObjectFromPool<T>() where T : MonoBehaviour, ItemT
        {
            var obj = Pool[typeof(T)].Get().GetComponent<T>();
            obj.IsDestroied = false;
            _activeObjects.Add(obj);

            return obj;
        }

        protected void ReturnObjectToPoolAt(int idx)
        {
            var obj = _activeObjects[idx];
            _activeObjects.RemoveAt(idx);

            Pool[obj.GetType()].Release(obj.gameObject);
        }

        /// <summary>
        /// 활성화 오브젝트 목록에서 오브젝트를 제거합니다.
        /// </summary>
        /// <param name="item">제거할 오브젝트</param>
        protected bool RemoveObject(ItemT item)
        {
            return _activeObjects.Remove(item);
        }

        /// <summary>
        /// 활성화 오브젝트 목록에서 인덱스에 해당하는 오브젝트를 제거합니다.
        /// </summary>
        /// <param name="idx">제거할 오브젝트의 인덱스</param>
        protected void RemoveObject(int idx)
        {
            _activeObjects.RemoveAt(idx);
        }
    }
}
