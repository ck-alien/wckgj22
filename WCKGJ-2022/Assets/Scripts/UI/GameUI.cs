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

        protected override void Start()
        {
            base.Start();

            SetLifeBar(3);
            _scoreText.text = FormatScoreText(1243423535);

            _settingsButton.OnClickAsObservable()
                .Subscribe(_ => GameManager.Instance.IsPaused.Value = true);
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

        private static string FormatScoreText(uint score)
        {
            return $"{score:#,0}";
        }
    }
}
