using System;
using System.Collections.Generic;
using EarthIsMine.Object;
using UnityEngine;
using UnityEngine.Pool;

namespace EarthIsMine.Pool
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField]
        private Enemy[] _poolingPrefabs;

        public IReadOnlyDictionary<Type, IObjectPool<Enemy>> Pool => _pool;

        private readonly Dictionary<Type, GameObject> _poolingPrefabsCache = new();
        private readonly Dictionary<Type, IObjectPool<Enemy>> _pool = new();

        public IObjectPool<Enemy> this[Type enemyType] => _pool[enemyType];

        private void Awake()
        {
            foreach (var enemy in _poolingPrefabs)
            {
                var prefab = enemy.gameObject;
                var enemyType = enemy.GetType();
                var pool = GameObjectPool<Enemy>.Create(enemy);

                _poolingPrefabsCache.TryAdd(enemyType, prefab);
                _pool.TryAdd(enemyType, pool);
            }
        }
    }
}
