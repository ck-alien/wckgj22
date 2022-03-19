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

        protected FMOD.Studio.EventInstance instance;

        protected virtual void Awake()
        {
            UITransform = gameObject.GetComponent<RectTransform>();
            UITransform.anchoredPosition = Vector2.zero;

            instance = FMODUnity.RuntimeManager.CreateInstance("event:/UiClick");


            CanvasGroup = gameObject.GetComponent<CanvasGroup>();

            gameObject.SetActive(IsDefault);
        }

        protected virtual void Start()
        {
        }
    }
}
