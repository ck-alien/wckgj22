using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField, Tooltip("일정 시간 후 부여되는 점수")]
        public int TimeScore { get; private set; }

        [field: SerializeField, Tooltip("일정 시간 후 점수를 주는 간격")]
        public float TimeScoreInterval { get; private set; }
    }
}
