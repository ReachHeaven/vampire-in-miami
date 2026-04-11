using System;
using Base.Player;
using Cysharp.Threading.Tasks;
using Test;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public Player player;
    
    private void Start()
    {
        Init().Forget();
    }

    private async UniTask Init()
    {
        Debug.Log($"Init: GameMain");
        var test = CMS.Get<CMSTest>();
        G.HudView.SetMessage("Survive 10 minutes");
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        G.HudView.ClearMessage();

        G.HudView.SetMessage("Wave 1 incoming...");
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        G.HudView.ClearMessage();
        Debug.Log($"Found: {test != null}, id: {test?.id}");
    }
}