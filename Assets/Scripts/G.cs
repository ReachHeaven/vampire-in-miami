using UI;
using UnityEngine;

public static class G
{
    public static GameMain GameMain;
    public static HudView HudView;
    public static SceneLoader SceneLoader;
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
            Debug.Log(G.GameMain);
            Debug.Log(G.HudView);
        };
        
        if (G.GameMain == null) Debug.LogError("Bootstrap: GameMain not found in scene");
        if (G.HudView == null) Debug.LogError("Bootstrap: HudView not found in scene");
    }

    private static T CreateSimpleService<T>() where T : Component, IService
    {
        GameObject g = new GameObject(typeof(T).ToString());

        g.transform.parent = serviceHolder.transform;
        T t = g.AddComponent<T>();
        t.Init();
        return g.GetComponent<T>();
    }
}

public interface IService
{
    public void Init();
}