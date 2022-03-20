using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace EarthIsMine.Jobs
{
    [BurstCompile]
    public struct ObjectMoveJob : IJobParallelForTransform
    {
        [ReadOnly]
        public float DeltaTime;

        [ReadOnly]
        public NativeArray<float> Speeds;

        [ReadOnly]
        public Vector3 MoveDirection;

        public void Execute(int index, TransformAccess transform)
        {
            var speed = Speeds[index];
            var position = transform.position;
            position += DeltaTime * speed * MoveDirection;
            transform.position = position;
        }
    }
}
