using System;
using Base.Player;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        G.GameMain = this;
        G.Hud = FindFirstObjectByType<HudView>();
    }

    private void Start()
    {
        G.Hud.SetHealth(
            G.Player.State.Tag<TagStats>().MaxHealth,
            G.Player.State.Health);
        G.Waves.RunAll().Forget();
    }


    private void Update()
    {
        Keyboard kb =  Keyboard.current;
        if (kb.zKey.wasPressedThisFrame)
        {
            G.Waves.KillAllEnemies();
        }
    }

    public void OnAllWavesCleared()
    {
        G.Hud.SetMessage("All waves cleared");
        Debug.Log("[GameMain] All waves cleared!");
    }


}
