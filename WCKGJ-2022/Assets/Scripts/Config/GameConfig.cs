using UnityEngine;

namespace EarthIsMine.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField, Min(0f)]
        public int TargetFrameRate { get; private set; }
    }
}
