using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace EarthIsMine.Jobs
{
    [BurstCompile]
    public struct EnemyCollsionCheckJob : IJobParallelFor
    {
        public struct JobResult
        {
            public CollisionJobResults Result;
            public int OtherIndex;

            public JobResult(CollisionJobResults result, int otherIndex = -1)
            {
                Result = result;
                OtherIndex = otherIndex;
            }
        }

        [ReadOnly]
        public NativeArray<Bounds> Enemies;

        [ReadOnly]
        public NativeArray<Bounds> Projectiles;

        [ReadOnly]
        public Bounds Player;

        [WriteOnly]
        public NativeArray<JobResult> Output;

        public void Execute(int index)
        {
            var enemy = Enemies[index];

            if (CollisionCheck.IsCollide2D(enemy, Player))
            {
                Output[index] = new JobResult(CollisionJobResults.CollideWithPlayer);
                return;
            }

            for (int i = 0; i < Projectiles.Length; i++)
            {
                var projectile = Projectiles[i];
                if (CollisionCheck.IsCollide2D(enemy, projectile))
                {
                    Output[index] = new JobResult(CollisionJobResults.CollideWithProjectile, i);
                    return;
                }
            }

            Output[index] = new JobResult(CollisionJobResults.NotCollide);
        }
    }
}
