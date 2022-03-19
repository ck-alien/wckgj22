using UnityEngine;

namespace EarthIsMine.Pool
{
    public class ReturnToPool : MonoBehaviour
    {
        public IGameObjectPool Pool { get; set; }

        public void Return()
        {
            Pool.Release(gameObject);
        }
    }
}
