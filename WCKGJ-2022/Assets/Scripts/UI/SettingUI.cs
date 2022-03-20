using System;
using System.Collections.Generic;
using EarthIsMine.Manager;
using EarthIsMine.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace EarthIsMine.UI
{
    public class SettingUI : Presenter
    {
        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private TMP_Dropdown _resolutionDropdown;

        [SerializeField]
        private Toggle _fullScreenToggle;

        [SerializeField]
        private Slider _masterVolumeSlider;

        [SerializeField]
        private Slider _bgmVolumeSlider;

        [SerializeField]
        private Slider _sfxVolumeSlider;

        [SerializeField]
        private Slider _uiSfxVolumeSlider;

        private Resolution[] _resolutions;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            _backButton.OnClickAsObservable()
                .Subscribe(_ => UIManager.Instance.Show<TitleUI>());

            _resolutionDropdown.onValueChanged.AsObservable()
                .Subscribe(v =>
                {
                    var resolution = _resolutions[v];
                    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
                });

            _fullScreenToggle.onValueChanged.AsObservable()
                .Subscribe(v => Screen.fullScreen = v);

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
            InitResolutionDropdown(_resolutionDropdown);
            _fullScreenToggle.isOn = Screen.fullScreen;

            _masterVolumeSlider.value = GameSound.MasterVolume;
            _bgmVolumeSlider.value = GameSound.BGMVolume;
            _sfxVolumeSlider.value = GameSound.SFXVolume;
            _uiSfxVolumeSlider.value = GameSound.UISFXVolume;
        }

        private void InitResolutionDropdown(TMP_Dropdown dropdown)
        {
            _resolutions = new Resolution[Screen.resolutions.Length];
            Array.Copy(Screen.resolutions, _resolutions, Screen.resolutions.Length);

            var options = new List<TMP_Dropdown.OptionData>();

            var width = Screen.width;
            var height = Screen.height;

            var idx = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                var resolution = _resolutions[i];
                options.Add(new TMP_Dropdown.OptionData($"{resolution.width}x{resolution.height}"));

                if (resolution.width == width && resolution.height == height)
                {
                    Debug.Log($"{i}: {resolution.width}x{resolution.height}");
                    idx = i;
                }
            }

            dropdown.options = options;
            dropdown.value = idx;
        }
    }
}
