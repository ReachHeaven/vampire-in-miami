using Base.Player;
using UI;

public static class G
{
    public static GameMain GameMain;
    public static PlayerView Player;
    public static HudView Hud;
    public static WaveRunner Waves;
    public static string PlayerName;

    public static readonly EnemyFactory EnemyFactory = new();
}