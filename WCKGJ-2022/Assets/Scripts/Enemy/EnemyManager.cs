using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct EnemyJob : IJobParallelForTransform
{
    public float Time { get; set; }

    public void Execute(int index, TransformAccess transform)
    {
        var position = transform.position;
        position.y = math.sin(Time);
        transform.position = position;
    }
}

public class EnemyManager : MonoBehaviour
{
    [field: SerializeField]
    public Transform[] Transforms { get; set; }

    private TransformAccessArray _transforms;

    private void Awake()
    {
        _transforms = new TransformAccessArray(Transforms);
    }

    private void Update()
    {
        var job = new EnemyJob { Time = Time.time };
        job.Schedule(_transforms);
    }

    private void OnDestroy()
    {
        _transforms.Dispose();
    }
}
