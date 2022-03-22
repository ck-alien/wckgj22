using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField, Tooltip("체력")]
        private int _life = 10;

        [SerializeField, Tooltip("이동 속도")]
        private float _speed = 1f;

        [SerializeField, Tooltip("처치 시 점수")]
        private int _score = 1;

        [SerializeField, Tooltip("피격 효과음")]
        private FMODUnity.EventReference _hitSound;

        [SerializeField, Tooltip("파괴 효과음")]
        private FMODUnity.EventReference _destroySound;

        public int Life => _life;
        public float Speed => _speed;
        public int Score => _score;
        public FMODUnity.EventReference HitSound => _hitSound;
        public FMODUnity.EventReference DestroySound => _destroySound;
    }
}
