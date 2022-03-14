using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, GameInputAction.IPlayerActions
{
    private void Start()
    {
        GameInput.Actions.Player.SetCallbacks(this);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Fire");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Move -> {context.ReadValue<Vector2>()}");
    }
}
