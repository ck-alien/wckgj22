using System.Collections;
using UnityEngine;

namespace EarthIsMine.Player
{
    public class PlayerDamaged : MonoBehaviour
    {
        [SerializeField] private float _invincibleTime;
        [SerializeField] private int _spriteCount;
        private SpriteRenderer _spriteRenderer;

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

        private IEnumerator FaidOutInCoroutine()// 캐릭터가 공격당할시 깜빡거리는 효과
        {
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
                    Debug.Log("Change");
                }
            }
        }

    }
}
