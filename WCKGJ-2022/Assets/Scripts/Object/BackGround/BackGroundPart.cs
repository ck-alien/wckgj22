using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EarthIsMine.Object
{
    public class BackGroundPart : MonoBehaviour
    {
        private Transform tr;

        [SerializeField] private Vector3 _startPos;

        [SerializeField] private float _limitY;


        [SerializeField] private SpriteRenderer[] _sprites;

        SpriteRenderer currentSprite;

        private void Awake()
        {
            tr = GetComponent<Transform>();
            _sprites = GetComponentsInChildren<SpriteRenderer>();
            currentSprite = _sprites[0];
        }

        private void Start()
        {
            Default();

        }

        private void Default()
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                if (currentSprite != _sprites[i])
                {
                    _sprites[i].gameObject.SetActive(false);
                }
            }
        }

        public void ChangeSprite(Day currentTime)
        {
            StartCoroutine(ChangeSpriteCoroutine((int)currentTime));
        }


        private IEnumerator ChangeSpriteCoroutine(int index)
        {
            SpriteRenderer nextSprite = _sprites[index];
            if (currentSprite == nextSprite)
            {
                yield break;
            }
            currentSprite.sortingOrder = -1;
            nextSprite.sortingOrder = -2;
            _sprites[index].gameObject.SetActive(true);
            nextSprite.color = new Color(1, 1, 1, 1);
            float time = 0;
            _sprites[index] = nextSprite;
            while (true)
            {
                yield return null;
                Color tmp = currentSprite.color;
                time += Time.deltaTime;
                tmp.a = Mathf.Lerp(1, 0, time);
                currentSprite.color = tmp;
                if (currentSprite.color.a == 0)
                {
                    currentSprite.gameObject.SetActive(false);
                    currentSprite = _sprites[index];
                    break;
                }
            }

            currentSprite = _sprites[index];

            yield return null;
        }

        private void Update()
        {
            if (tr.position.y < _limitY)
            {
                tr.position = _startPos;
            }
        }



    }
}
