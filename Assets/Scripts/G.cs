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
}

[DefaultExecutionOrder(-9999)]
public static class GameBootstrapper
{
    private static GameObject serviceHolder;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        Debug.Log("Bootstrap: started");
        CMS.Init();
        Debug.Log("Bootstrap: CMS initialized");
        
        serviceHolder = new GameObject("[Services]"); 
        Object.DontDestroyOnLoad(serviceHolder);

        G.SceneLoader = CreateSimpleService<SceneLoader>();
        
        G.SceneLoader.OnLoad += () =>
        {
            G.GameMain = Object.FindFirstObjectByType<GameMain>();
            G.HudView = Object.FindFirstObjectByType<HudView>();
            G.WaveSystem = CreateSimpleService<WaveSystem>();
            G.HealthSystem = CreateSimpleService<HealthSystem>();
            G.SceneLoader = CreateSimpleService<SceneLoader>();
            G.GameMain.Init();
        };
    }

    private static T CreateSimpleService<T>() where T : Component, IService
    {
        GameObject g = new GameObject(typeof(T).ToString());

        g.transform.parent = serviceHolder.transform;
        T t = g.AddComponent<T>();
        Debug.Log($"CreateSimpleService: created GameObject '{g.name}', component type = {t.GetType()}, is MonoBehaviour = {t is MonoBehaviour}");
        t.Init();
        return g.GetComponent<T>();
    }
}

public interface IService
{
    public void Init();
}