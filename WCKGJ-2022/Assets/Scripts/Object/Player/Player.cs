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
        [SerializeField]
        private int _defaultLife = 3;

        public ReactiveProperty<int> Life { get; set; }

        public PlayerController Controller { get; private set; }
        public PlayerAttack Attack { get; private set; }
        public PlayerDamaged Damaged { get; private set; }

        private void Awake()
        {
            Controller = GetComponent<PlayerController>();
            Attack = GetComponent<PlayerAttack>();
            Damaged = GetComponent<PlayerDamaged>();

            Life = new ReactiveProperty<int>(_defaultLife);
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

            Life.Value -= 1;
            Damaged.OnHit();
        }

        private void OnDead()
        {
            Controller.enabled = false;
            Attack.enabled = false;

            gameObject.SetActive(false);
        }
    }
}
