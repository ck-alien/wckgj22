using UnityEngine;

namespace EarthIsMine.Object
{
    public class ObjectMove : MonoBehaviour
    {
        [field: SerializeField]
        public Vector3 Direction { get; set; }

        public float Speed { get; set; }

        private void Update()
        {
            gameObject.transform.position += Time.deltaTime * Speed * Direction;
        }
    }
}
