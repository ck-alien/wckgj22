using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField, Tooltip("발사체 속도")]
        private float _speed = 20f;

        [field: SerializeField, Tooltip("대미지")]
        private int _damage = 1;

        [field: SerializeField, Tooltip("발사 효과음")]
        private FMODUnity.EventReference _shotSound;

        public float Speed => _speed;
        public int Damage => _damage;
        public FMODUnity.EventReference ShotSound => _shotSound;
    }
}
