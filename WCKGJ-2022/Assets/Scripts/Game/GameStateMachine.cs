using EarthIsMine.FSM;

namespace EarthIsMine.Game
{
    public enum StageType
    {
        Day, Night
    }

    public class GameStateMachine : StateMachine
    {
        public StageType StageType { get; set; }
    }
}
