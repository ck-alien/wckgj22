using System.Linq;
using EarthIsMine.Jobs;
using EarthIsMine.Object;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace EarthIsMine.Manager
{
    /// <summary>
    /// 플레이어 혹은 몬스터가 만들어내는 모든 발사체를 관리하는 Manager 클래스이다.
    /// </summary>
    public class ProjectileManager : ObjectManager<ProjectileManager, Projectile>
    {
        [SerializeField]
        private int _destroyPositionY = 6;

        /*
        private void Update()
        {
            var projectiles = ActiveObjects;
            var enemies = EnemyManager.Instance.ActiveObjects;

            if (projectiles.Count == 0 || enemies.Count == 0)
            {
                return;
            }

            using var projectileBounds = new NativeArray<Bounds>(projectiles.Count, Allocator.TempJob);
            using var enemyBounds = new NativeArray<Bounds>(enemies.Count, Allocator.TempJob);

            using var results = new NativeArray<CollisionJobResults>(projectiles.Count, Allocator.TempJob);

            projectileBounds.Fill(projectiles, p => p.GetComponent<BoxCollider2D>().bounds);
            enemyBounds.Fill(enemies, e => e.GetComponent<BoxCollider2D>().bounds);

            var job = new ProjectileCollisionCheckJob
            {
                Projectiles = projectileBounds,
                Enemies = enemyBounds,
                Results = results
            };
            var handle = job.Schedule(projectileBounds.Length, 1);
            handle.Complete();

            for (int i = 0; i < projectiles.Count; i++)
            {
                var projectile = projectiles[i];
                var result = results[i];

                switch (result)
                {
                    case CollisionJobResults.CollideWithEnemy:
                        projectile.IsDestroied = true;
                        break;

                    default:
                        break;
                }
            }
        }
        */

        private void LateUpdate()
        {
            var projectiles = ActiveObjects;
            var projectilesAlive = projectiles.Where(p => !p.IsDestroied).ToArray();

            if (projectilesAlive.Length == 0)
            {
                return;
            }

            using var transforms = new TransformAccessArray(projectilesAlive.Length);
            using var speeds = new NativeArray<float>(projectilesAlive.Length, Allocator.TempJob);

            speeds.Fill(projectilesAlive, p =>
            {
                transforms.Add(p.transform);
                return p.Speed;
            });

            var job = new ObjectMoveJob
            {
                DeltaTime = Time.deltaTime,
                Speeds = speeds,
                MoveDirection = Vector3.up
            };

            var handle = job.Schedule(transforms);
            handle.Complete();

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = projectiles[i];
                if (projectile.IsDestroied || projectile.transform.position.y >= _destroyPositionY)
                {
                    RemoveProjectile(i);
                }
            }
        }

        public void Create<T>(Vector3 position, int count) where T : Projectile
        {
            if (count < 1)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var p = GetObjectFromPool<T>();
                p.transform.position = position;
            }

            if (count == 1)
            {
                var p = GetObjectFromPool<T>();
                p.transform.position = position;
            }
        }

        private void RemoveProjectile(int idx)
        {
            ActiveObjects[idx].Remove();
            ReturnObjectToPoolAt(idx);
        }
    }
}
