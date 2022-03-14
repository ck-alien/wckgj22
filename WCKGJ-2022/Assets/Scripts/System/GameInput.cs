using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameInput : Singleton<GameInput>
{
    /// <summary>
    /// Provides input actions in game.<para/>
    /// This property will initialize after Awake.
    /// </summary>
    public static GameInputAction Actions { get; private set; }

    /// <summary>
    /// Provides PlayerInput controls in game.<para/>
    /// This property will initialize after Awake.
    /// </summary>
    public static PlayerInput Controls { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitInstance()
    {
        CreateNewSingletonObject("Game Input");
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        Actions = new GameInputAction();

        var controls = gameObject.AddComponent<PlayerInput>();
        controls.actions = Actions.asset;
        controls.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        Controls = controls;
    }

    private void OnEnable()
    {
        Actions.Player.Enable();
    }

    private void OnDisable()
    {
        Actions.Player.Disable();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Actions.Dispose();
    }
}
