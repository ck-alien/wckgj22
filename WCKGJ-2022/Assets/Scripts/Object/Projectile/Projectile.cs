using EarthIsMine.Data;
using EarthIsMine.Manager;
using EarthIsMine.Pool;
using UnityEngine;


namespace EarthIsMine.Object
{
    public interface IProjectile : IObject
    {
        public ProjectileTypes Type { get; }

        public void Remove();
    }

    [RequireComponent(typeof(ObjectMove))]
    public abstract class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private FMODUnity.EventReference _shotSound;

        [field: SerializeField]
        public ProjectileData Data { get; private set; }

        public abstract ProjectileTypes Type { get; }

        private void Start()
        {
            GetComponent<ObjectMove>().Speed = GameManager.Instance.Player.Data.MoveSpeed;
        }

        private void OnEnable()
        {
            if (!_shotSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(_shotSound);
            }
        }

        protected virtual void Update()
        {
            if (transform.position.y >= ProjectileManager.Instance.DestroyPositionY)
            {
                Remove();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Remove();
            }
        }

        public void Remove()
        {
            if (gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }
    }
}
