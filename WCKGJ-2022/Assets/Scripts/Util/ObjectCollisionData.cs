using UnityEngine;

public struct ObjectCollisionData
{
    public Vector3 Position { get; set; }
    public Bounds Bounds { get; set; }

    public ObjectCollisionData(Vector3 position, Bounds bounds)
    {
        Position = position;
        Bounds = bounds;
    }
}
