using UnityEngine;

public class Bootstrap
{
    private static bool _initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        if (_initialized) return;
        _initialized = true;

        CMS.Unload();
        CMS.Init();

        G.PlayerName = PlayerPrefs.GetString("PlayerName", string.Empty);
    }
}