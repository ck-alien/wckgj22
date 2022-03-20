using EarthIsMine.Manager;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class GameOverUI : Presenter
    {
        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _titleButton;

        [SerializeField]
        private Button _closeButton;

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.IsGameOver.Where(s => s is true)
                .Subscribe(_ => UIManager.Instance.Show<GameOverUI>(true));

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("GameScene"));

            _titleButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("MainScene"));

            _closeButton.OnClickAsObservable()
                .Subscribe(_ => Application.Quit());
        }
    }
}
