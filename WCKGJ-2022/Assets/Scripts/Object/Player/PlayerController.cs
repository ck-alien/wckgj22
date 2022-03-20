using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EarthIsMine.Object
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; set; } = 3f;

        [field: SerializeField]
        public Vector2 ClampPosition { get; private set; } = new Vector2(3.5f, 4f);

        private Vector2 _direction;

        private void LateUpdate()
        {
            var position = transform.position;
            position += Speed * Time.deltaTime * new Vector3(_direction.x, _direction.y, transform.position.z);
            position.x = math.clamp(position.x, -ClampPosition.x, ClampPosition.x);
            position.y = math.clamp(position.y, -ClampPosition.y, ClampPosition.y);
            transform.position = position;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
        }
    }
}
