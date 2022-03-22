using EarthIsMine.Manager;
using UnityEngine;

namespace EarthIsMine.Object
{
    /// <summary>
    /// 플레이어의 공격을 담당하는 클래스이다. 직접적인 공격 호출과 쿨타임을 계산하는 역할을 한다.
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("발사체가 시작하는 위치를 나타낸다.")]
        private Transform _startPos;

        [field: SerializeField]
        public ProjectileTypes ProjectileType { get; set; }

        public int ShotCount { get; set; } = 1;

        public float Interval { get; set; } = 0.5f;

        private float _elapsedTime;

        private void OnEnable()
        {
            _elapsedTime = 0f;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= Interval)
            {
                ProjectileManager.Instance.Create(ProjectileType, _startPos.position, ShotCount);
                _elapsedTime = 0f;
            }
        }
    }
}
