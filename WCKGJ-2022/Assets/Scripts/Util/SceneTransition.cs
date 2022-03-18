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

    private Vector2 _size;

    private void Awake()
    {
        var w = Screen.width;
        var h = Screen.height;

        var diameter = Mathf.Sqrt((w * w) + (h * h));
        _size = new Vector2(diameter, diameter);

        _start.gameObject.SetActive(false);
        _end.gameObject.SetActive(false);
        _background.gameObject.SetActive(false);
    }

    public UniTask Open(float duraion, Ease ease)
    {
        return DOTween.Sequence()
            .AppendCallback(() => _start.sizeDelta = Vector2.zero)
            .AppendCallback(() => _start.gameObject.SetActive(true))
            .Append(_start.DOSizeDelta(_size, duraion).SetEase(ease))
            .AppendCallback(() => _background.gameObject.SetActive(true))
            .AppendCallback(() => _start.gameObject.SetActive(false))
            .ToUniTask();
    }

    public UniTask Close(float duraion, Ease ease)
    {
        return DOTween.Sequence()
            .AppendCallback(() => _end.sizeDelta = Vector2.zero)
            .AppendCallback(() => _end.gameObject.SetActive(true))
            .AppendCallback(() => _background.gameObject.SetActive(false))
            .Append(_end.DOSizeDelta(_size, duraion).SetEase(ease))
            .AppendCallback(() => _end.gameObject.SetActive(false))
            .ToUniTask();
    }
}
