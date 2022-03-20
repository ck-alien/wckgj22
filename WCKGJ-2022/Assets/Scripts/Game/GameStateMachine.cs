using EarthIsMine.FSM;
using UnityEngine;

namespace EarthIsMine.Game
{
    public enum StageTypes
    {
        Day, Night
    }

    public class GameStateMachine : StateMachine
    {
        [field: SerializeField]
        public Vector2 EnemySpawnPosition { get; private set; }

        [field: SerializeField]
        public Vector2 EnemySpawnDistance { get; private set; }

        public StageTypes StageType { get; set; }
    }
}
