using UnityEngine;

namespace EarthIsMine.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class Presenter : MonoBehaviour
    {
        [field: SerializeField]
        public bool IsDefault { get; private set; }

        public RectTransform UITransform { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }

        protected virtual void Awake()
        {
            UITransform = gameObject.GetComponent<RectTransform>();
            UITransform.anchoredPosition = Vector2.zero;

            CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            gameObject.SetActive(IsDefault);
        }
    }
}
