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
        Init();
    }

    private void Init()
    {
        Debug.Log($"Init: GameMain");
        var test = CMS.Get<CMSTest>();
        G.HudView.SetMessage("Survive 10 minutes");
        G.HudView.ClearMessage();

        G.HudView.SetMessage("Wave 1 incoming...");
        Debug.Log($"Found: {test != null}, id: {test?.id}");
    }
}