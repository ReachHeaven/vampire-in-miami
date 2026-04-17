using Base.Player;
using UI;
using UnityEngine;

public static class G
{
    public static GameMain GameMain;
    public static PlayerView Player;
    public static Arena Arena;
    public static HudView Hud;
    public static WaveRunner Waves;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot() => CMS.Init();
}