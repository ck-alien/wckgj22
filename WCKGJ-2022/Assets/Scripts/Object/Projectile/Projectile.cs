using UnityEngine;


namespace EarthIsMine.Object
{
    public interface IProjectile : IJobObject
    {
        public ProjectileTypes Type { get; }
        public float Speed { get; set; }

        public void Remove();
    }

    public abstract class Projectile : MonoBehaviour, IProjectile
    {
        public abstract ProjectileTypes Type { get; }
        public bool IsDestroied { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        public void Remove()
        {
        }
    }
}
