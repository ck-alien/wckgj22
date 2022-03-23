using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EarthIsMine.UI
{
    public class CreditUI : Presenter
    {
        [field: SerializeField]
        private TextMeshProUGUI[] textList;

        [SerializeField]
        private float _durableTime;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            StartCoroutine(Show());
            if (GameInput.Instance)
            {
                GameInput.Instance.ActionMaps.Game.Pause.performed += OnClose;
            }
        }

        private void OnDisable()
        {
            StopCoroutine(Show());
            if (GameInput.Instance)
            {
                GameInput.Instance.ActionMaps.Game.Pause.performed -= OnClose;
            }
        }

        private void OnClose(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SceneLoader.Instance.Load("MainScene");
            }
        }

        private IEnumerator Show()
        {
            for (int i = 0; i < textList.Length; i++)
            {
                float time = 0;
                int start = 0; int end = 1;
                int cnt = 0;
                Color alpha = textList[i].color;
                alpha.a = 0;
                while (true)
                {
                    time += Time.deltaTime;
                    alpha.a = Mathf.Lerp(start, end, time / 2);
                    textList[i].color = alpha;
                    if (alpha.a == end && cnt == 0)
                    {
                        Debug.Log("Fade ON");
                        cnt++;
                        start += end;
                        end = start - end;
                        start -= end;
                        yield return new WaitForSeconds(_durableTime);
                        time = 0;
                    }
                    else if (alpha.a == end && cnt == 1)
                    {
                        break;
                    }
                    yield return null;
                }
            }
            SceneLoader.Instance.Load("MainScene");
        }
    }
}
