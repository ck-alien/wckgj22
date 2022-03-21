using System.Collections;
using EarthIsMine.Data;
using EarthIsMine.Manager;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class GameOverUI : Presenter
    {
        [SerializeField]
        private float _textAnimateInterval = 0.2f;

        [SerializeField]
        private ResultMessageData _resultMessageData;

        [SerializeField]
        private TextMeshProUGUI _resultText;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _titleButton;

        [SerializeField]
        private Button _closeButton;

        protected override void Awake()
        {
            base.Awake();

            _resultText.text = string.Empty;

            GameManager.Instance.IsGameOver.Where(s => s is true)
                .Subscribe(_ =>
                {
                    UIManager.Instance.Show<GameOverUI>(true);
                    SetResultText(GameManager.Instance.Score.Value);
                });
        }

        protected override void Start()
        {
            base.Start();

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("GameScene"));

            _titleButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("MainScene"));

            _closeButton.OnClickAsObservable()
                .Subscribe(_ => Application.Quit());
        }

        private void SetResultText(int score)
        {
            var messages = _resultMessageData.Messages;

            for (int i = messages.Length - 1; i >= 0; i--)
            {
                if (score >= messages[i].Score)
                {
                    MainThreadDispatcher.StartUpdateMicroCoroutine(AnimateText(messages[i].Message));
                    return;
                }
            }

            MainThreadDispatcher.StartUpdateMicroCoroutine(AnimateText("GOOD~"));
        }

        private IEnumerator AnimateText(string text, float delay = 0.3f)
        {
            var time = 0f;
            while (time < delay)
            {
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;

            for (int i = 0; i < text.Length; i++)
            {
                _resultText.text = text[..i];

                while (time < _textAnimateInterval)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
                time = 0f;
            }
        }
    }
}
