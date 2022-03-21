using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Manager
{
    public class EnemyManager : ObjectManager<EnemyManager, Enemy, EnemyTypes>
    {
        [SerializeField]
        private float _destroyPositionY = -5;

        public float DestroyPositionY => _destroyPositionY;

        public Enemy Spawn(EnemyTypes enemyType, Vector3 position)
        {
            var enemy = Pools[enemyType].Take();
            enemy.transform.position = position;
            return enemy.GetComponent<Enemy>();
        }
    }
}
