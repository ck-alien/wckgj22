using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Manager
{
    /// <summary>
    /// 플레이어 혹은 몬스터가 만들어내는 모든 발사체를 관리하는 Manager 클래스이다.
    /// </summary>
    public class ProjectileManager : ObjectManager<ProjectileManager, Projectile, ProjectileTypes>
    {
        [SerializeField]
        private float _destroyPositionY = 6;

        public float DestroyPositionY => _destroyPositionY;

        public void Create(ProjectileTypes projectileType, Vector3 position, int count)
        {
            if (count < 1)
            {
                return;
            }

            var pool = Pools[projectileType];

            if (count == 1)
            {
                var p = pool.Take();
                p.transform.position = position;
            }
            // count가 2 이상일 때 발사체 생성 로직 구현
        }
    }
}
