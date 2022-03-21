using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class PlayerDamaged : MonoBehaviour
    {
        [SerializeField] private float _invincibleTime;
        [SerializeField] private int _spriteCount;
        [SerializeField] private FMODUnity.EventReference _hitSound;

        private SpriteRenderer _spriteRenderer;

        public bool IsInvincible { get; set; }

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

            if (!_hitSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(_hitSound);
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
                time += Time.deltaTime / (_invincibleTime / _spriteCount / 2);
                fadeColor.a = Mathf.Lerp(start, end, time);
                _spriteRenderer.color = fadeColor;
                if (_spriteRenderer.color.a == end)
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