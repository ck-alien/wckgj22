using EarthIsMine.Manager;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace EarthIsMine.UI
{
    public class TitleUI : Presenter
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _settingButton;

        [SerializeField]
        private Button _creditButton;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            _startButton.OnClickAsObservable().Subscribe(_ =>
            {
                CanvasGroup.interactable = false;
                SceneLoader.Instance.Load("GameScene");
            });

            _settingButton.OnClickAsObservable()
                .Subscribe(_ => UIManager.Instance.Show<SettingUI>());

            _creditButton.OnClickAsObservable().Subscribe(_ =>
            {
                CanvasGroup.interactable = false;
                SceneLoader.Instance.Load("CreditScene");
            });
        }
    }
}
