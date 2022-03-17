using UnityEngine;

namespace EarthIsMine.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class Presenter : MonoBehaviour
    {
        public RectTransform UITransform { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }

        private void Awake()
        {
            UITransform = gameObject.GetComponent<RectTransform>();
            UITransform.anchoredPosition = Vector2.zero;

            CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        }
    }
}
