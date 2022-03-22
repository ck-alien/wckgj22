using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EarthIsMine.System
{
    public sealed class GameSound : Singleton<GameSound>
    {
        public const string MasterVolumePrefKey = "mastervolume";
        public const string BGMVolumePrefKey = "bgmvolume";
        public const string SFXVolumePrefKey = "sfxvolume";
        public const string UISFXVolumePrefKey = "uisfxvolume";

        [field: SerializeField, InspectorName("Master Volume"), Range(0f, 1f)]
        public static float MasterVolume { get; set; }

        [field: SerializeField, InspectorName("BGM Volume"), Range(0f, 1f)]
        public static float BGMVolume { get; set; }

        [field: SerializeField, InspectorName("SFX Volume"), Range(0f, 1f)]
        public static float SFXVolume { get; set; }

        [field: SerializeField, InspectorName("UI SFX Volume"), Range(0f, 1f)]
        public static float UISFXVolume { get; set; }

        private VCA _masterVCA;
        private VCA _bgmVCA;
        private VCA _sfxVCA;
        private VCA _uiSfxVCA;

        private float _cachedMasterVolume;
        private float _cachedBGMVolume;
        private float _cachedSFXVolume;
        private float _cachedUISFXVolume;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitInstance()
        {
            CreateNewSingletonObject("Game Sound");
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            float volume;
            if (TryGetVCAController("Master", out _masterVCA))
            {
                _masterVCA.getVolume(out volume);
                MasterVolume = volume;
            }
            if (TryGetVCAController("BGM", out _bgmVCA))
            {
                _bgmVCA.getVolume(out volume);
                BGMVolume = volume;
            }
            if (TryGetVCAController("SFX", out _sfxVCA))
            {
                _sfxVCA.getVolume(out volume);
                SFXVolume = volume;
            }
            if (TryGetVCAController("UI-SFX", out _uiSfxVCA))
            {
                _uiSfxVCA.getVolume(out volume);
                UISFXVolume = volume;
            }

            MasterVolume = PlayerPrefs.GetFloat(MasterVolumePrefKey, 0.8f);
            BGMVolume = PlayerPrefs.GetFloat(BGMVolumePrefKey, 1f);
            SFXVolume = PlayerPrefs.GetFloat(SFXVolumePrefKey, 1f);
            UISFXVolume = PlayerPrefs.GetFloat(UISFXVolumePrefKey, 1f);
        }

        private void LateUpdate()
        {
            SetVolume(_masterVCA, MasterVolumePrefKey, ref _cachedMasterVolume, MasterVolume);
            SetVolume(_bgmVCA, BGMVolumePrefKey, ref _cachedBGMVolume, BGMVolume);
            SetVolume(_sfxVCA, SFXVolumePrefKey, ref _cachedSFXVolume, SFXVolume);
            SetVolume(_uiSfxVCA, UISFXVolumePrefKey, ref _cachedUISFXVolume, UISFXVolume);
        }

        private bool TryGetVCAController(string name, out VCA vca)
        {
            try
            {
                vca = RuntimeManager.GetVCA($"vca:/{name}");
                return vca.isValid();
            }
            catch (VCANotFoundException)
            {
                Debug.LogWarning($"{name}을 찾을 수 없습니다.");
                vca = default;
                return false;
            }
        }

        private void SetVolume(VCA vca, string prefKey, ref float cachedVolume, float newVolume)
        {
            if (!vca.isValid() || newVolume == cachedVolume)
            {
                return;
            }

            if (vca.setVolume(newVolume) == FMOD.RESULT.OK)
            {
                cachedVolume = newVolume;
                PlayerPrefs.SetFloat(prefKey, newVolume);
            }
        }
    }
}
