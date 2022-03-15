using EarthIsMine.Object;
using EarthIsMine.Pool;
using UnityEngine;
using UnityEngine.Pool;

namespace EarthIsMine.Manager
{
    [RequireComponent(typeof(EnemyPool))]
    public class EnemyManager : Singleton<EnemyManager>
    {
        private EnemyPool _pool;

        private void Start()
        {
            _pool = GetComponent<EnemyPool>();
        }

        public PooledObject<Enemy> SpawnEnemy<T>(out Enemy spawnedEnemy) where T : Enemy
        {
            return _pool[typeof(T)].Get(out spawnedEnemy);
        }
    }
}
