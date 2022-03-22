using EarthIsMine.Data;
using EarthIsMine.Manager;
using EarthIsMine.Pool;
using UnityEngine;


namespace EarthIsMine.Object
{
    public interface IProjectile : IObject
    {
        public ProjectileTypes Type { get; }

        public void Remove(bool showEffect);
    }

    [RequireComponent(typeof(ObjectMove))]
    public abstract class Projectile : MonoBehaviour, IProjectile
    {
        [field: SerializeField]
        public ProjectileData Data { get; private set; }

        public abstract ProjectileTypes Type { get; }

        private void OnEnable()
        {
            GetComponent<ObjectMove>().Speed = Data.Speed;

            if (!Data.ShotSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(Data.ShotSound);
            }
        }

        protected virtual void Update()
        {
            if (transform.position.y >= ProjectileManager.Instance.DestroyPositionY)
            {
                Remove(false);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Remove(true);
            }
        }

        public void Remove(bool showEffect = true)
        {
            if (showEffect)
            {
                ParticleManager.Instance.Play("projectile-hit", transform.position);
            }
            if (gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }
    }
}
