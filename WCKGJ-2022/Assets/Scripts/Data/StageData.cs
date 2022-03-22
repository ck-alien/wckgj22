using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
    public class StageData : ScriptableObject
    {
        [SerializeField, Tooltip("추가된 순서대로 웨이브가 진행됩니다.")]
        private WaveData[] _waves;

        public WaveData[] Waves => _waves;
    }
}

