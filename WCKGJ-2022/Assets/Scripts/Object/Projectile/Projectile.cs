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
        [SerializeField]
        private FMODUnity.EventReference _shotSound;

        public abstract ProjectileTypes Type { get; }
        public bool IsDestroied { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        private void OnEnable()
        {
            if (!_shotSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(_shotSound);
            }
        }

        public void Remove()
        {
        }
    }
}
