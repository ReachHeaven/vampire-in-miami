using Systems;
using UI;
using UnityEngine;

public static class G
{
    public static GameMain GameMain;
    public static HudView HudView;
    public static SceneLoader SceneLoader;
    public static WaveSystem WaveSystem;
    public static HealthSystem HealthSystem;
    public static FightSystem FightSystem;
}

[DefaultExecutionOrder(-9999)]
public static class GameBootstrapper
{
    private static GameObject serviceHolder;
    private static bool _servicesCreated;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        _servicesCreated = false;
        
        CMS.Init();


        serviceHolder = new GameObject("[Services]");
        Object.DontDestroyOnLoad(serviceHolder);

        G.SceneLoader = CreateSimpleService<SceneLoader>();

        G.SceneLoader.OnLoad += () =>
        {
            G.GameMain = Object.FindFirstObjectByType<GameMain>();
            G.HudView = Object.FindFirstObjectByType<HudView>();

            if (!_servicesCreated)
            {
                G.WaveSystem = CreateSimpleService<WaveSystem>();
                G.HealthSystem = CreateSimpleService<HealthSystem>();
                G.FightSystem = CreateSimpleService<FightSystem>();
                _servicesCreated = true;
            }

            G.GameMain.Init();
        };
    }

    private static T CreateSimpleService<T>() where T : Component, IService
    {
        var go = new GameObject(typeof(T).Name);
        go.transform.parent = serviceHolder.transform;
        var service = go.AddComponent<T>();
        service.Init();
        return service;
    }
}

public interface IService
{
    void Init();
}