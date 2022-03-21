using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoaderConfig", menuName = "Config/SceneLoaderConfig")]
public class SceneLoaderConfig : ScriptableObject
{
    [field: SerializeField]
    public SceneTransition LoadingUIPrefab { get; private set; }

    [field: SerializeField, Min(0f)]
    public float Delay { get; private set; }

    [field: SerializeField, Min(0.1f)]
    public float TransitionDuration { get; private set; }

    [field: SerializeField]
    public Ease EaseType { get; private set; }
}
