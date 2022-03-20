using EarthIsMine.Manager;
using UnityEngine;

namespace EarthIsMine.Object
{
    public interface IEnemy : IJobObject
    {
        public EnemyTypes Type { get; }
        public int Life { get; set; }

        public void Kill();
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField]
        private int _addScore;

        public abstract EnemyTypes Type { get; }

        public bool IsDestroied { get; set; }

        [field: SerializeField]
        public int DefaultLife { get; set; }

        public int Life
        {
            get => _life;
            set
            {
                // Debug.Log(_life);
                _life = Mathf.Max(0, value);
                if (_life == 0)
                {
                    GameManager.Instance.Score.Value += _addScore;
                    IsDestroied = true;
                }
                else
                {
                    _hitAnimCount = 2;
                }
            }
        }

        private int _life;
        private SpriteRenderer _renderer;
        private float _time;
        private int _hitAnimCount;

        protected virtual void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            Life = DefaultLife;
        }

        protected virtual void Update()
        {
            if (_hitAnimCount > 0)
            {
                _time += Time.deltaTime;
                if (_time > 0.1f)
                {
                    _renderer.color = _hitAnimCount % 2 == 0 ? Color.red : Color.white;
                    _hitAnimCount--;
                }
                else
                {
                    return;
                }
            }

            _time = 0f;
        }

        protected virtual void OnDisable()
        {
            _renderer.material.color = Color.white;
            _hitAnimCount = 0;
            _time = 0f;
        }

        public void Kill()
        {
        }
    }
}
