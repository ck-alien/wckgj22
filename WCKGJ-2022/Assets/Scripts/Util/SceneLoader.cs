using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : Singleton<SceneLoader>
{
    public static readonly string ConfigPath = "Config/SceneLoaderConfig";

    public SceneLoaderConfig Config { get; private set; }

    private SceneTransition _transition;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnInitialize()
    {
        CreateNewSingletonObject("Scene Loader");
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        Config = Resources.Load<SceneLoaderConfig>(ConfigPath);
        _transition = Instantiate(Config.LoadingUIPrefab, transform);
    }

    public IObservable<Unit> Load(string sceneName)
    {
        var coroutine = Observable.FromMicroCoroutine(() => LoadProcess(sceneName));
        coroutine.Subscribe().AddTo(gameObject);

        return coroutine;
    }

    private IEnumerator LoadProcess(string sceneName)
    {
        var openTransition = _transition.Open(Config.TransitionDuration, Config.EaseType)
            .ToObservable()
            .ToYieldInstruction();

        while (!openTransition.IsDone)
        {
            yield return null;
        }

        var task = SceneManager.LoadSceneAsync(sceneName);
        var elapsedTime = 0f;
        while (!task.isDone || elapsedTime < Config.Delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;

        var closeTransition = _transition.Close(Config.TransitionDuration, Config.EaseType)
            .ToObservable()
            .ToYieldInstruction();

        while (!closeTransition.IsDone)
        {
            yield return null;
        }
    }
}
