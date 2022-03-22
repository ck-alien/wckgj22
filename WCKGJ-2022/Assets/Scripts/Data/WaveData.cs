using System;
using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Data
{
    [Serializable]
    public class WavePattern
    {
        [SerializeField, Tooltip("적 타입")]
        private EnemyTypes _enemyType;

        [SerializeField, Tooltip("좌측 하단부터 시작하는 상대적인 스폰 위치")]
        private Vector2 _relativePosition;

        public EnemyTypes EnemyType => _enemyType;
        public Vector2 RelativePosition => _relativePosition;
    }

    [CreateAssetMenu(fileName = "WaveData", menuName = "Data/WaveData")]
    public class WaveData : ScriptableObject
    {
        [SerializeField, Tooltip("웨이브 진행 시 적 생성 패턴")]
        private WavePattern[] _spawnPattern;

        public WavePattern[] SpawnPattern => _spawnPattern;
    }
}
