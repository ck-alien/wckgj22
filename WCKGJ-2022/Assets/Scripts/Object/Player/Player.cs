using EarthIsMine.Data;
using EarthIsMine.Manager;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Object
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerAttack))]
    [RequireComponent(typeof(PlayerDamaged))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerData Data { get; private set; }

        public ReactiveProperty<int> Life { get; set; }

        public PlayerController Controller { get; private set; }
        public PlayerAttack Attack { get; private set; }
        public PlayerDamaged Damaged { get; private set; }

        private void Awake()
        {
            Controller = GetComponent<PlayerController>();
            Controller.Parent = this;

            Attack = GetComponent<PlayerAttack>();
            Attack.Parent = this;

            Damaged = GetComponent<PlayerDamaged>();
            Damaged.Parent = this;

            Life = new ReactiveProperty<int>(Data.DefaultLife);
        }

        private void Start()
        {
            Life.SkipLatestValueOnSubscribe().Subscribe(life => Debug.Log($"PlayerLife: {life}"));

            GameManager.Instance.IsGameOver
                .Where(s => s is true)
                .Subscribe(_ => OnDead());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Hit();
            }
        }

        public void Hit(bool ignoreInvincible = false)
        {
            if ((!ignoreInvincible && Damaged.IsInvincible) || GameManager.Instance.IsGameOver.Value)
            {
                return;
            }

            Damaged.OnHit();
            Life.Value -= 1;
        }

        private void OnDead()
        {
            Controller.enabled = false;
            Attack.enabled = false;

            ParticleManager.Instance.Play("player-destroy", transform.position);
            if (!Data.DestroySound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(Data.DestroySound);
            }

            gameObject.SetActive(false);
        }
    }
}
