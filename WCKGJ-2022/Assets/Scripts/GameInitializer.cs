using EarthIsMine.Config;
using UnityEngine;

namespace EarthIsMine
{
    public static class GameInitializer
    {
        public static readonly string ConfigPath = "Config/GameConfig";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeGame()
        {
            var config = Resources.Load<GameConfig>(ConfigPath);

            Application.targetFrameRate = config.TargetFrameRate;
        }
    }
}
