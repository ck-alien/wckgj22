using EarthIsMine.Manager;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;


namespace EarthIsMine.UI
{
    public class TitleUI : Presenter
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _settingButton;

        protected override void Start()
        {
            base.Start();

            _startButton.OnClickAsObservable().Subscribe(_ =>
            {
                CanvasGroup.interactable = false;
                SceneLoader.Instance.Load("GameScene");
                instance.setVolume(EarthIsMine.System.GameSound.UISFXVolume);
                instance.start();
            });

            _settingButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    UIManager.Instance.Show<SettingUI>();
                    instance.setVolume(EarthIsMine.System.GameSound.UISFXVolume);
                    instance.start();
                });
        }
    }
}
