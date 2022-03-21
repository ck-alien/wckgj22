using System;
using EarthIsMine.Object;
using UnityEngine;

namespace EarthIsMine.Data
{
    [Serializable]
    public struct EnemyPatternDataItem
    {
        [field: SerializeField]
        public EnemyTypes EnemyType { get; private set; }

        [field: SerializeField, Tooltip("좌측 하단부터 시작하는 상대적인 스폰 위치입니다.")]
        public Vector2 RelativePosition { get; private set; }
    }

    [CreateAssetMenu(fileName = "EnemyPatternData", menuName = "Data/EnemyPatternData")]
    public class EnemyPatternData : ScriptableObject
    {
        [field: SerializeField]
        public EnemyPatternDataItem[] SpawnPattern { get; private set; }
    }
}
