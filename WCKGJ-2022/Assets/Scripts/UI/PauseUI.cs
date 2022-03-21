using EarthIsMine.Manager;
using EarthIsMine.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class PauseUI : Presenter
    {
        [SerializeField]
        private Button _resumeButton;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _titleButton;

        [SerializeField]
        private Button _quitButton;

        [SerializeField]
        private Slider _masterVolumeSlider;

        [SerializeField]
        private Slider _bgmVolumeSlider;

        [SerializeField]
        private Slider _sfxVolumeSlider;

        [SerializeField]
        private Slider _uiSfxVolumeSlider;

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.IsPaused.Subscribe(isPaused => gameObject.SetActive(isPaused));
        }

        protected override void Start()
        {
            base.Start();

            _resumeButton.OnClickAsObservable()
                .Subscribe(_ => GameManager.Instance.IsPaused.Value = false);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("GameScene"));

            _titleButton.OnClickAsObservable()
                .Subscribe(_ => SceneLoader.Instance.Load("MainScene"));

            _quitButton.OnClickAsObservable()
                .Subscribe(_ => Application.Quit());

            _masterVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(v => GameSound.MasterVolume = v);

            _bgmVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(v => GameSound.BGMVolume = v);

            _sfxVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(v => GameSound.SFXVolume = v);

            _uiSfxVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(v => GameSound.UISFXVolume = v);
        }

        private void OnEnable()
        {
            _masterVolumeSlider.value = GameSound.MasterVolume;
            _bgmVolumeSlider.value = GameSound.BGMVolume;
            _sfxVolumeSlider.value = GameSound.SFXVolume;
            _uiSfxVolumeSlider.value = GameSound.UISFXVolume;
        }
    }
}
