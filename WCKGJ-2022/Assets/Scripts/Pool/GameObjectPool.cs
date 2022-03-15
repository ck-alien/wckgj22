using UnityEngine;
using UnityEngine.Pool;

namespace EarthIsMine.Pool
{
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        public static IObjectPool<T> Create(T prefab)
        {
            return new ObjectPool<T>(
                () => CreateNewPoolItem(prefab.gameObject), OnTakeFromPool, OnReturnToPool, OnDestroyItem);
        }

        private static T CreateNewPoolItem(GameObject prefab)
        {
            var newObject = UnityEngine.Object.Instantiate(prefab);
            return newObject.GetComponent<T>();
        }

        private static void OnTakeFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        private static void OnReturnToPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        private static void OnDestroyItem(T item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }
}
