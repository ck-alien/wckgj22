using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Player
{
    public class PlayerDamaged : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public float _invincibleTime;

        public int _spriteCount;
        private bool _invincible;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void GetDamaged(int damage)
        {
            if (!_invincible)
            {
                StartCoroutine(DamagedCoroutine());
            }
        }


        private IEnumerator DamagedCoroutine()
        {
            _invincible = true;
            StartCoroutine(FaidOutInCoroutine());
            _invincible = false;
            yield return null;
        }

        private IEnumerator FaidOutInCoroutine()
        {
            float time = 0;
            float start = 1; float end = 0;
            Color _fadeColor = _spriteRenderer.color;
            int count = 0;
            while (count < _spriteCount * 2)
            {
                time += Time.deltaTime / ((_invincibleTime / _spriteCount) / 2);
                _fadeColor.a = Mathf.Lerp(start, end, time);
                _spriteRenderer.color = _fadeColor;
                yield return null;
                if (_spriteRenderer.color.a == end)
                {
                    start = start + end;
                    end = start - end;
                    start = start - end;
                    time = 0;
                    count++;
                    Debug.Log("Change");
                }
            }
        }

    }
}
