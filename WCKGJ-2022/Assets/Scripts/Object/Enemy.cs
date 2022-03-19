using UnityEngine;

namespace EarthIsMine.Object
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField]
        private int _life;

        public int Life
        {
            get => _life;
            set
            {
                Debug.Log(_life);
                _life = Mathf.Max(0, value);
                if (_life == 0)
                {
                    IsDestroied = true;
                }
                else
                {
                    _hitAnimCount = 2;
                }
            }
        }

        public bool IsDestroied { get; set; }

        private SpriteRenderer _renderer;
        private float _time;
        private int _hitAnimCount;

        protected virtual void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
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
