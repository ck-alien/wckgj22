using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "StageWaveData", menuName = "Data/StageWaveData")]
    public class StageWaveData : ScriptableObject
    {
        [field: SerializeField]
        public EnemyPatternData[] Waves { get; private set; }
    }
}
