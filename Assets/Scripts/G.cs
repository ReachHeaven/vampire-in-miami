using UnityEngine;

public static class G
{
    public static GameMain GameMain;
}

[DefaultExecutionOrder(-9999)]
public class GameBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        G.GameMain = Object.FindFirstObjectByType<GameMain>();
    }
}