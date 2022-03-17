using System;
using DG.Tweening;
using UnityEngine;

namespace EarthIsMine.UI
{
    [Serializable]
    public struct AnimationInfo<T>
    {
        [field: SerializeField]
        public T Target { get; private set; }

        [field: SerializeField]
        public bool DoFade { get; private set; }

        [field: SerializeField]
        public Vector2 StartPosition { get; private set; }

        [field: SerializeField]
        public Vector2 StartScale { get; private set; }

        [field: SerializeField, Min(0f)]
        public float WaitTime { get; private set; }

        [field: SerializeField, Min(0f)]
        public float Duration { get; private set; }

        [field: SerializeField]
        public Ease EaseType { get; private set; }

        [field: NonSerialized]
        public Vector2 DefaultPosition { get; set; }

        [field: NonSerialized]
        public Vector2 DefaultScale { get; set; }
    }

    public class AnimationOnEnable : MonoBehaviour
    {
        [SerializeField]
        private AnimationInfo<RectTransform>[] _animationInfos;

        private void Awake()
        {
            for (int i = 0; i < _animationInfos.Length; i++)
            {
                SetDefault(ref _animationInfos[i]);
            }

            static void SetDefault(ref AnimationInfo<RectTransform> info)
            {
                var target = info.Target;

                if (target.gameObject.GetComponent<CanvasGroup>() == null)
                {
                    target.gameObject.AddComponent<CanvasGroup>();
                }

                info.DefaultPosition = target.anchoredPosition;
                info.DefaultScale = new Vector2(target.localScale.x, target.localScale.z);
            }
        }

        private void OnEnable()
        {
            foreach (var info in _animationInfos)
            {
                Animate(in info);
            }
        }

        private static void Animate(in AnimationInfo<RectTransform> info)
        {
            var transform = info.Target;
            var group = transform.gameObject.GetComponent<CanvasGroup>();

            transform.anchoredPosition = info.DefaultPosition + info.StartPosition;
            transform.localScale = ToVector3Scale(info.StartScale);
            group.alpha = info.DoFade ? 0f : 1f;

            DOTween.Sequence()
                .SetLink(info.Target.gameObject, LinkBehaviour.KillOnDisable)
                .AppendInterval(info.WaitTime)
                .Append(transform.DOAnchorPos(info.DefaultPosition, info.Duration).SetEase(info.EaseType))
                .Join(transform.DOScale(ToVector3Scale(info.DefaultScale), info.Duration).SetEase(info.EaseType))
                .Join(group.DOFade(1f, info.Duration).SetEase(info.EaseType));

            static Vector3 ToVector3Scale(Vector2 scale) => new(scale.x, 1, scale.y);
        }
    }
}
