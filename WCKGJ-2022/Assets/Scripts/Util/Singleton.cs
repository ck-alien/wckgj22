using UnityEngine;

/// <summary>
/// Awake를 통해 생성되는 싱글톤입니다.
/// </summary>
/// <typeparam name="T">싱글톤을 사용하려는 컴포넌트</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 현재 씬에 존재하는 싱글톤 오브젝트입니다.
    /// </summary>
    public static T Instance { get; private set; }
    public static string SingletonName { get; } = typeof(T).Name;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<T>();
        }
        else
        {
            Debug.LogWarning($"Can not instantiate {SingletonName} singleton game object because it is already exist");

            // 한 오브젝트에 컴포넌트가 중복 추가된 경우도 있어서 같은 게임오브젝트인지 체크
            if (Instance.gameObject == gameObject)
            {
                Destroy(this);
                return;
            }
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    protected static T CreateNewSingletonObject(string name = null)
    {
        return new GameObject(name ?? SingletonName).AddComponent<T>();
    }
}
