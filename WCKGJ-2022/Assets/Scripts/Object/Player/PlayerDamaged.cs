using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class PlayerDamaged : PlayerBehaviour
    {
        [SerializeField]
        private int _spriteCount;

        public bool IsInvincible { get; set; }

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnHit()
        {
            if (IsInvincible)
            {
                return;
            }

            if (Parent.Data.HitSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(Parent.Data.HitSound);
            }

            Camera.main.DOShakePosition(0.2f, strength: 0.5f);

            Observable.FromMicroCoroutine(() => DamagedCoroutine())
                .Subscribe()
                .AddTo(gameObject);
        }

        private IEnumerator DamagedCoroutine()
        {
            IsInvincible = true;

            // 캐릭터가 공격당할시 깜빡거리는 효과
            float time = 0;
            float start = 1;
            float end = 0;
            Color fadeColor = _spriteRenderer.color;
            int count = 0;
            while (count < _spriteCount * 2)
            {
                yield return null;
                time += Time.deltaTime / (Parent.Data.InvincibleTime / _spriteCount / 2);
                fadeColor.g = Mathf.Lerp(start, end, time);
                fadeColor.b = Mathf.Lerp(start, end, time);
                _spriteRenderer.color = fadeColor;
                if (_spriteRenderer.color.g == end && _spriteRenderer.color.b == end)
                {
                    start += end;
                    end = start - end;
                    start -= end;
                    time = 0;
                    count++;
                }
            }

            IsInvincible = false;
        }
    }
}
