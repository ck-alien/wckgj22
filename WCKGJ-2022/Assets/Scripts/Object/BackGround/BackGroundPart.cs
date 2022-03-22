using UnityEngine;

namespace EarthIsMine.Object
{
    public class BackGroundPart : MonoBehaviour
    {
        [SerializeField] private float _limitY;

        private Vector3 _startPos;

        // [SerializeField] private SpriteRenderer[] _sprites;

        // private SpriteRenderer _currentSprite;

        /*
        private void Awake()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
            _currentSprite = _sprites[0];
        }
        */

        private void Start()
        {
            _startPos = transform.position;
        }

        private void Update()
        {
            if (transform.position.y <= _limitY)
            {
                transform.position = _startPos;
            }
        }

        /*
        private void Start()
        {
            Default();
        }

        private void Default()
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                if (_currentSprite != _sprites[i])
                {
                    _sprites[i].gameObject.SetActive(false);
                }
            }
        }
        */

        /*
        public void ChangeSprite(Day currentTime)
        {
            StartCoroutine(ChangeSpriteCoroutine((int)currentTime));
        }


        private IEnumerator ChangeSpriteCoroutine(int index)
        {
            SpriteRenderer nextSprite = _sprites[index];
            if (_currentSprite == nextSprite)
            {
                yield break;
            }
            _currentSprite.sortingOrder = -1;
            nextSprite.sortingOrder = -2;
            _sprites[index].gameObject.SetActive(true);
            nextSprite.color = new Color(1, 1, 1, 1);
            float time = 0;
            _sprites[index] = nextSprite;
            while (true)
            {
                yield return null;
                Color tmp = _currentSprite.color;
                time += Time.deltaTime;
                tmp.a = Mathf.Lerp(1, 0, time);
                _currentSprite.color = tmp;
                if (_currentSprite.color.a == 0)
                {
                    _currentSprite.gameObject.SetActive(false);
                    _currentSprite = _sprites[index];
                    break;
                }
            }

            _currentSprite = _sprites[index];

            yield return null;
        }
        */
    }
}
