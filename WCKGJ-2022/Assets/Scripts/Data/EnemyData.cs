using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField]
        public int Life { get; private set; }

        [field: SerializeField]
        public float Speed { get; private set; }

        [field: SerializeField]
        public int Score { get; private set; }
    }
}
