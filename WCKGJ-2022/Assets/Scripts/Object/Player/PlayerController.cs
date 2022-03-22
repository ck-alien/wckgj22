using Unity.Mathematics;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class PlayerController : MonoBehaviour
    {
        public float Speed { get; set; } = 3f;

        [field: SerializeField]
        public Vector2 ClampPosition { get; private set; } = new Vector2(3.5f, 4f);

        private void LateUpdate()
        {
            var dir = GameInput.Instance.ActionMaps.Player.Move.ReadValue<Vector2>();

            var position = transform.position;
            position += Speed * Time.deltaTime * new Vector3(dir.x, dir.y, transform.position.z);
            position.x = math.clamp(position.x, -ClampPosition.x, ClampPosition.x);
            position.y = math.clamp(position.y, -ClampPosition.y, ClampPosition.y);
            transform.position = position;
        }
    }
}
