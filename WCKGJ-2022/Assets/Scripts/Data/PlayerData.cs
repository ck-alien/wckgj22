using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField, Tooltip("이동 속도")]
        private float _moveSpeed = 6f;

        [SerializeField, Tooltip("기본 생명")]
        private int _defaultLife = 3;

        [SerializeField, Tooltip("피격 후 무적 시간")]
        private float _invincibleTime = 0.5f;

        [SerializeField, Tooltip("공격 시 발사체 갯수 (미구현)")]
        private int _shotCount = 1;

        [SerializeField, Tooltip("공격 간격")]
        private float _shotInterval = 0.1f;

        [SerializeField, Tooltip("피격 효과음")]
        private FMODUnity.EventReference _hitSound;

        [SerializeField, Tooltip("파괴 효과음")]
        private FMODUnity.EventReference _destroySound;

        public float MoveSpeed => _moveSpeed;
        public int DefaultLife => _defaultLife;
        public float InvincibleTime => _invincibleTime;
        public int ShotCount => _shotCount;
        public float ShotInterval => _shotInterval;
        public FMODUnity.EventReference HitSound => _hitSound;
        public FMODUnity.EventReference DestroySound => _destroySound;
    }
}
