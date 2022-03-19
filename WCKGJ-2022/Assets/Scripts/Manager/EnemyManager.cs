using System;
using System.Collections.Generic;
using EarthIsMine.Object;
using EarthIsMine.Pool;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace EarthIsMine.Manager
{
    [BurstCompile]
    public struct EnemyCollsionCheckJob : IJobParallelFor
    {
        public enum JobResults
        {
            NotCollide,
            CollideWithPlayer,
            CollideWithProjectile
        }

        [ReadOnly]
        public NativeArray<Bounds> Enemies;

        [ReadOnly]
        public NativeArray<Bounds> Projectiles;

        [ReadOnly]
        public Bounds Player;

        [WriteOnly]
        public NativeArray<JobResults> Results;

        public void Execute(int index)
        {
            var enemy = Enemies[index];

            if (CollisionCheck.IsCollide2D(enemy, Player))
            {
                Results[index] = JobResults.CollideWithPlayer;
                return;
            }

            foreach (var projectile in Projectiles)
            {
                if (CollisionCheck.IsCollide2D(enemy, projectile))
                {
                    Results[index] = JobResults.CollideWithProjectile;
                    return;
                }
            }

            Results[index] = JobResults.NotCollide;
        }
    }

    [BurstCompile]
    public struct EnemyMoveJob : IJobParallelForTransform
    {
        [ReadOnly]
        public float DeltaTime;

        [ReadOnly]
        public float Speed;

        public void Execute(int index, TransformAccess transform)
        {
            var position = transform.position;
            position += DeltaTime * Speed * Vector3.down;
            transform.position = position;
        }
    }

    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField]
        private Enemy[] _enemyPrefabs;

        [SerializeField]
        private int _destroyPositionY = -5;

        [field: SerializeField]
        public float MoveSpeed { get; set; }

        public IReadOnlyCollection<IEnemy> Enemies => _activeEnemies;

        private readonly Dictionary<Type, GameObject> _poolingPrefabsCache = new();
        private readonly Dictionary<Type, IGameObjectPool> _pool = new();
        private readonly LinkedList<Enemy> _activeEnemies = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var enemy in _enemyPrefabs)
            {
                var prefab = enemy.gameObject;
                var enemyType = enemy.GetType();
                var pool = new GameObjectPool(prefab);

                _poolingPrefabsCache.TryAdd(enemyType, prefab);
                _pool.TryAdd(enemyType, pool);
            }
        }

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

            var activeProjectiles = ProjectileManager.Instance.ProjectileInfo._poolingOn;
            var playerBounds = GameManager.Instance.Player.GetComponent<BoxCollider2D>().bounds;

            using var enemyBounds = new NativeArray<Bounds>(_activeEnemies.Count, Allocator.TempJob);
            using var projectileBounds = new NativeArray<Bounds>(activeProjectiles.Count, Allocator.TempJob);
            using var results = new NativeArray<EnemyCollsionCheckJob.JobResults>(enemyBounds.Length, Allocator.TempJob);

            NativeArrayUtil.Fill(enemyBounds, _activeEnemies, e => e.gameObject.GetComponent<BoxCollider2D>().bounds);
            NativeArrayUtil.Fill(projectileBounds, activeProjectiles, p => p.gameObject.GetComponent<BoxCollider2D>().bounds);

            // 다른 Update에서 Enemy 목록에 접근해야 하기 때문에 체크만 함
            var job = new EnemyCollsionCheckJob
            {
                Enemies = enemyBounds,
                Projectiles = projectileBounds,
                Player = playerBounds,
                Results = results
            };
            var handle = job.Schedule(enemyBounds.Length, 1);
            handle.Complete();

            var enemyNode = _activeEnemies.First;
            for (int i = 0; i < results.Length; i++)
            {
                var result = results[i];
                var enemy = enemyNode.Value;

                switch (result)
                {
                    case EnemyCollsionCheckJob.JobResults.CollideWithPlayer:
                        enemy.IsDestroied = true;
                        break;

                    case EnemyCollsionCheckJob.JobResults.CollideWithProjectile:
                        enemy.Life -= 1;
                        break;

                    case EnemyCollsionCheckJob.JobResults.NotCollide:
                    default:
                        break;
                }

                enemyNode = enemyNode.Next;
            }
        }

        private void LateUpdate()
        {
            using var transforms = new TransformAccessArray(_activeEnemies.Count);

            // 삭제는 마지막에 모두 모아서 진행
            foreach (var enemy in _activeEnemies)
            {
                if (!enemy.IsDestroied)
                {
                    transforms.Add(enemy.transform);
                }
            }

            var job = new EnemyMoveJob
            {
                DeltaTime = Time.deltaTime,
                Speed = MoveSpeed
            };

            var handle = job.Schedule(transforms);
            handle.Complete();

            _activeEnemies.ForEach(enemy =>
            {
                if (enemy.IsDestroied || enemy.transform.position.y <= _destroyPositionY)
                {
                    KillEnemy(enemy);
                }
            });
        }

        public T Spawn<T>(Vector3 position) where T : Enemy
        {
            var pool = _pool[typeof(T)];
            var enemy = pool.Get().GetComponent<T>();
            enemy.IsDestroied = false;

            enemy.gameObject.transform.position = position;
            if (!enemy.gameObject.TryGetComponent<ReturnToPool>(out var returnToPool))
            {
                returnToPool = enemy.gameObject.AddComponent<ReturnToPool>();
            }

            returnToPool.Pool = pool;

            _activeEnemies.AddLast(enemy);
            return enemy;
        }

        private void KillEnemy(Enemy enemy)
        {
            enemy.Kill();
            _activeEnemies.Remove(enemy);

            var pool = _pool[enemy.GetType()];
            pool.Release(enemy.gameObject);
        }
    }
}
