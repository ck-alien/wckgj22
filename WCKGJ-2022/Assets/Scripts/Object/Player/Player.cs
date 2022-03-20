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
            Life.SkipLatestValueOnSubscribe().Subscribe(life =>
                {
                    Debug.Log($"PlayerLife: {life}");
                    if (life > 0)
                    {
                        Damaged.OnHit();
                    }
                });
        }

        public void Hit()
        {
            if (Damaged.IsInvincible)
            {
                return;
            }

            Life.Value -= 1;
        }
    }
}
