using System.Linq;
using EarthIsMine.Jobs;
using EarthIsMine.Object;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace EarthIsMine.Manager
{
    public class EnemyManager : ObjectManager<EnemyManager, Enemy>
    {
        [SerializeField]
        private int _destroyPositionY = -5;

        [field: SerializeField]
        public float MoveSpeed { get; set; }

        private void Update()
        {
            /*
             * [update]
             * collision check
             * [late update]
             * remove objects
             * move objects
             * 
             * Enemy와 Projectile의 경우
             * collision check 및 move objects 모두 각자 매니저에서 진행, 이후 삭제는 LateUpdate에서 진행
             */

            var enemies = ActiveObjects;
            var projectiles = ProjectileManager.Instance.ActiveObjects;
            var player = GameManager.Instance.Player;
            if (enemies.Count == 0)
            {
                return;
            }

            using var enemyBounds = new NativeArray<Bounds>(enemies.Count, Allocator.TempJob);
            using var projectileBounds = new NativeArray<Bounds>(projectiles.Count, Allocator.TempJob);
            var playerBounds = player.GetComponent<BoxCollider2D>().bounds;

            using var output = new NativeArray<EnemyCollsionCheckJob.JobResult>(enemies.Count, Allocator.TempJob);

            enemyBounds.Fill(enemies, e => e.GetComponent<BoxCollider2D>().bounds);
            projectileBounds.Fill(projectiles, p => p.GetComponent<BoxCollider2D>().bounds);

            // 다른 Update에서 Enemy 목록에 접근해야 하기 때문에 체크만 함
            var job = new EnemyCollsionCheckJob
            {
                Enemies = enemyBounds,
                Projectiles = projectileBounds,
                Player = playerBounds,
                Output = output
            };
            var handle = job.Schedule(enemyBounds.Length, 1);
            handle.Complete();

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                var result = output[i];

                switch (result.Result)
                {
                    case CollisionJobResults.CollideWithPlayer:
                        player.Hit();
                        enemy.IsDestroied = true;
                        break;

                    case CollisionJobResults.CollideWithProjectile:
                        enemy.Life -= 1;
                        var idx = result.OtherIndex;
                        var other = projectiles[idx];
                        other.IsDestroied = true;
                        break;

                    default:
                        break;
                }
            }
        }

        private void LateUpdate()
        {
            var enemies = ActiveObjects;
            if (enemies.Count == 0)
            {
                return;
            }

            var enemiesAlive = enemies.Where(e => !e.IsDestroied).ToArray();

            using var transforms = new TransformAccessArray(enemiesAlive.Length);
            using var speeds = new NativeArray<float>(enemiesAlive.Length, Allocator.TempJob);

            speeds.Fill(enemiesAlive, e =>
            {
                transforms.Add(e.transform);
                return MoveSpeed;
            });

            var job = new ObjectMoveJob
            {
                DeltaTime = Time.deltaTime,
                Speeds = speeds,
                MoveDirection = Vector3.down
            };

            var handle = job.Schedule(transforms);
            handle.Complete();

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];
                if (enemy.IsDestroied || enemy.transform.position.y <= _destroyPositionY)
                {
                    KillEnemy(i);
                }
            }
        }

        public T Spawn<T>(Vector3 position) where T : Enemy
        {
            var enemy = GetObjectFromPool<T>();
            enemy.transform.position = position;

            return enemy;
        }

        private void KillEnemy(int idx)
        {
            print($"kill enemy [{idx}]");
            ActiveObjects[idx].Kill();
            ReturnObjectToPoolAt(idx);
        }
    }
}
