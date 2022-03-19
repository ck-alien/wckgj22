using UnityEngine;
using UnityEngine.Pool;

namespace EarthIsMine.Pool
{
    public interface IGameObjectPool : IObjectPool<GameObject>
    {
    }

    public class GameObjectPool : ObjectPool<GameObject>, IGameObjectPool
    {
        public GameObjectPool(GameObject prefab)
            : base(() => CreateNewPoolItem(prefab), OnTakeFromPool, OnReturnToPool, OnDestroyItem) { }

        private static GameObject CreateNewPoolItem(GameObject prefab)
        {
            return UnityEngine.Object.Instantiate(prefab);
        }

        private static void OnTakeFromPool(GameObject item)
        {
            item.SetActive(true);
        }

        private static void OnReturnToPool(GameObject item)
        {
            item.SetActive(false);
        }

        private static void OnDestroyItem(GameObject item)
        {
            UnityEngine.Object.Destroy(item);
        }
    }
}
