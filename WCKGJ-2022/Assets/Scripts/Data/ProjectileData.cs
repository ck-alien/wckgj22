using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField]
        public float Speed { get; private set; }

        [field: SerializeField]
        public int Damage { get; private set; }
    }
}