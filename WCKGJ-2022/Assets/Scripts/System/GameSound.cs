using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameSound : Singleton<GameSound>
{
    public const string MasterVolumePrefKey = "master-volume";
    public const string BGMVolumePrefKey = "bgm-volume";
    public const string SFXVolumePrefKey = "sfx-volume";
    public const string UISFXVolumePrefKey = "uisfx-volume";

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

    [RuntimeInitializeOnLoadMethod]
    private static void InitInstance()
    {
        CreateNewSingletonObject();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        float volume;
        if (TryGetVCAController("MasterVolume", out _masterVCA))
        {
            _masterVCA.getVolume(out volume);
            MasterVolume = volume;
        }
        if (TryGetVCAController("BGMVolume", out _bgmVCA))
        {
            _bgmVCA.getVolume(out volume);
            BGMVolume = volume;
        }
        if (TryGetVCAController("SFXVolume", out _sfxVCA))
        {
            _sfxVCA.getVolume(out volume);
            SFXVolume = volume;
        }
        if (TryGetVCAController("UISFXVolume", out _uiSfxVCA))
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
        SetVolume(_masterVCA, ref _cachedMasterVolume, MasterVolume);
        SetVolume(_bgmVCA, ref _cachedBGMVolume, BGMVolume);
        SetVolume(_sfxVCA, ref _cachedSFXVolume, SFXVolume);
        SetVolume(_uiSfxVCA, ref _cachedUISFXVolume, UISFXVolume);
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

    private void SetVolume(VCA vca, ref float cachedVolume, float newVolume)
    {
        if (!vca.isValid() || newVolume == cachedVolume)
        {
            return;
        }

        if (vca.setVolume(newVolume) == FMOD.RESULT.OK)
        {
            cachedVolume = newVolume;
        }
    }
}
