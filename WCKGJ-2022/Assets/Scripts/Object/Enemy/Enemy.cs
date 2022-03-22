using System.Collections;
using EarthIsMine.Data;
using EarthIsMine.Manager;
using EarthIsMine.Pool;
using UniRx;
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

        /// <summary>
        /// 이 프로퍼티로 체력을 수정하면 Hit로 취급하여 각종 효과들이 발생합니다.
        /// </summary>
        public int Life
        {
            get => _life;
            set
            {
                _life = Mathf.Max(0, value);
                if (_life == 0)
                {
                    Kill();
                    GameManager.Instance.Score.Value += Data.Score;
                    return;
                }

                if (!Data.DestroySound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Data.HitSound, transform.position);
                }
                _hit = true;
            }
        }

        private SpriteRenderer _renderer;
        private int _life;
        private bool _hit;
        private bool _passed = false;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            GetComponent<ObjectMove>().Speed = Data.Speed;
        }

        private void OnEnable()
        {
            _life = Data.Life;
            _passed = false;
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

            if (!_passed && transform.position.y <= EnemyManager.Instance.DestroyPositionY)
            {
                Observable.FromMicroCoroutine(() => Pass(3f))
                    .Subscribe()
                    .AddTo(gameObject);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (_passed)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                Kill();
            }
            else if (other.CompareTag("Projectile"))
            {
                Life -= other.GetComponent<Projectile>().Data.Damage;
            }
        }

        public void Kill()
        {
            ParticleManager.Instance.Play("enemy-destroy", transform.position);
            if (!Data.DestroySound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(Data.DestroySound, transform.position);
            }

            if (gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }

        private IEnumerator Pass(float destroyDelay = 3f)
        {
            _passed = true;
            GameManager.Instance.Player.Hit(ignoreInvincible: true);

            var time = 0f;
            while (time < destroyDelay)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (gameObject && gameObject.TryGetComponent<ReturnToPool>(out var component))
            {
                component.Return();
            }
        }
    }
}
