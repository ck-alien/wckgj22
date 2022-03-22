using UnityEngine;

namespace EarthIsMine.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField, Tooltip("이동 속도")]
        public float MoveSpeed { get; private set; }

        [field: SerializeField, Tooltip("기본 생명")]
        public int DefaultLife { get; private set; }

        [field: SerializeField, Tooltip("피격 후 무적 시간")]
        public float InvincibleTime { get; private set; }

        [field: SerializeField, Tooltip("공격 간격")]
        public float ShotInterval { get; private set; }
    }
}
