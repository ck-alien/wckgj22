using System.Collections.Generic;
using EarthIsMine.Manager;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class GameUI : Presenter
    {
        [SerializeField]
        private GameObject _lifeBar;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private Button _settingsButton;

        private readonly Queue<GameObject> _life = new();


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            _settingsButton.OnClickAsObservable()
                .Subscribe(_ => GameManager.Instance.IsPaused.Value = true);

            GameManager.Instance.Player.Life.Subscribe(life => SetLifeBar(life));
            GameManager.Instance.Score.Subscribe(score => _scoreText.text = FormatScoreText(score));
        }

        private void SetLifeBar(int maxLife)
        {
            Debug.Assert(_lifeBar.transform.childCount >= maxLife, "Life Bar object count is less than max life!");

            _life.Clear();
            for (int i = 0; i < _lifeBar.transform.childCount; i++)
            {
                var child = _lifeBar.transform.GetChild(i).gameObject;
                if (i < maxLife)
                {
                    child.SetActive(true);
                    _life.Enqueue(child);
                }
                else
                {
                    child.SetActive(false);
                }
            }
        }

        private static string FormatScoreText(int score)
        {
            return $"{score:#,0}";
        }
    }
}
