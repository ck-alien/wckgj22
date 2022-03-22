using EarthIsMine.Data;
using EarthIsMine.Manager;
using EarthIsMine.Pool;
using UnityEngine;

namespace EarthIsMine.Object
{
    public interface IEnemy : IObject
    {
        public EnemyTypes Type { get; }
        public int Life { get; set; }

        public void Kill();
    }

    [RequireComponent(typeof(SpriteRenderer), typeof(ObjectMove))]
    public abstract class Enemy : MonoBehaviour, IEnemy
    {
        [field: SerializeField]
        public EnemyData Data { get; private set; }

        public abstract EnemyTypes Type { get; }

        public int Life
        {
            get => _life;
            set
            {
                // Debug.Log(_life);
                _life = Mathf.Max(0, value);
                if (_life == 0)
                {
                    Kill();
                    GameManager.Instance.Score.Value += Data.Score;
                    return;
                }
                _hit = true;
            }
        }

        private SpriteRenderer _renderer;
        private int _life;
        private bool _hit;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            GetComponent<ObjectMove>().Speed = Data.Speed;
        }

        private void OnEnable()
        {
            Life = Data.Life;
        }

        protected virtual void Update()
        {
            if (_hit)
            {
                _renderer.color = Color.red;
                _hit = false;
            }
            else
            {
                _renderer.color = Color.white;
            }

            if (transform.position.y <= EnemyManager.Instance.DestroyPositionY)
            {
                Kill();
                GameManager.Instance.Player.Hit(ignoreInvincible: true);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Kill();
            }
            else if (other.CompareTag("Projectile"))
            {
                var projectile = other.GetComponent<Projectile>();
                Life -= projectile.Data.Damage;
            }
        }

        public void Kill()
        {
            if (gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }
    }
}
