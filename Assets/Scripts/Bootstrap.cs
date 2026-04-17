using UnityEngine;

public class Bootstrap
{
    private static bool _initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        if (_initialized) return;
        Debug.Log("Bootstrapper: Initialization started");

        CMS.Unload();
        CMS.Init();
    }
}