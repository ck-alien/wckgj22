using System;
using System.Collections.Generic;
using EarthIsMine.Object;
using EarthIsMine.Pool;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public abstract class ObjectManager<ManagerT, ItemT, ItemTypesT> : Singleton<ManagerT>
        where ManagerT : MonoBehaviour
        where ItemT : MonoBehaviour, IObject
        where ItemTypesT : Enum
    {
        [Serializable]
        private class Prefabs
        {
            [field: SerializeField]
            public ItemTypesT Type { get; private set; }

            [field: SerializeField]
            public ItemT Prefab { get; private set; }
        }

        [SerializeField, Tooltip("오브젝트 프리팹 리스트")]
        private Prefabs[] _prefabs;

        protected IDictionary<ItemTypesT, IGameObjectPool> Pools => _pools;

        public int ActiveObjectsCount
        {
            get
            {
                var count = 0;
                foreach (var item in _pools.Values)
                {
                    count += item.CountActive;
                }
                return count;
            }
        }

        private readonly Dictionary<ItemTypesT, IGameObjectPool> _pools = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in _prefabs)
            {
                var pool = new GameObjectPool(item.Prefab.gameObject, 100);
                _pools.TryAdd(item.Type, pool);
            }
        }
    }
}
