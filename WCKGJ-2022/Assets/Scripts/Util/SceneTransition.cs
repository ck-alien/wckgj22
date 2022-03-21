using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private RectTransform _start;

    [SerializeField]
    private RectTransform _end;

    [SerializeField]
    private Image _background;

    private void Awake()
    {
        _start.gameObject.SetActive(false);
        _end.gameObject.SetActive(false);
        _background.gameObject.SetActive(false);
    }

    public UniTask Open(float duraion, Ease ease)
    {
        var size = GetSize();

        return DOTween.Sequence()
            .AppendCallback(() => _start.sizeDelta = Vector2.zero)
            .AppendCallback(() => _start.gameObject.SetActive(true))
            .Append(_start.DOSizeDelta(size, duraion).SetEase(ease))
            .AppendCallback(() => _background.gameObject.SetActive(true))
            .AppendCallback(() => _start.gameObject.SetActive(false))
            .ToUniTask();
    }

    public UniTask Close(float duraion, Ease ease)
    {
        var size = GetSize();

        return DOTween.Sequence()
            .AppendCallback(() => _end.sizeDelta = Vector2.zero)
            .AppendCallback(() => _end.gameObject.SetActive(true))
            .AppendCallback(() => _background.gameObject.SetActive(false))
            .Append(_end.DOSizeDelta(size, duraion).SetEase(ease))
            .AppendCallback(() => _end.gameObject.SetActive(false))
            .ToUniTask();
    }

    private static Vector2 GetSize()
    {
        var w = Screen.width;
        var h = Screen.height;

        var diameter = Mathf.Sqrt((w * w) + (h * h));
        var size = new Vector2(diameter * 2, diameter * 2);

        // print($"{w}, {h}, {diameter}, {size}");
        return size;
    }
}
