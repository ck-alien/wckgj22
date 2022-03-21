using UnityEngine;

namespace EarthIsMine
{
    public class GameInput : Singleton<GameInput>
    {
        public GameInputAction ActionMaps { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnIntialize()
        {
            CreateNewSingletonObject("Game Input");
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            ActionMaps = new GameInputAction();
        }

        private void OnEnable()
        {
            ActionMaps.Enable();
        }

        private void OnDisable()
        {
            ActionMaps.Disable();
        }
    }
}
