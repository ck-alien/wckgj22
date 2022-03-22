using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField, Tooltip("일정 시간 후 부여되는 점수")]
        private int _timeScore = 50;

        [SerializeField, Tooltip("일정 시간 후 점수를 주는 간격")]
        private float _timeScoreInterval = 1f;

        public int TimeScore => _timeScore;
        public float TimeScoreInterval => _timeScoreInterval;
    }
}
