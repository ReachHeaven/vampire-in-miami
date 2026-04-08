using System;
using Base.Player;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public Player player;
    private void Start()
    {
        Init();
    }

    private async void Init()
    {
        Debug.Log("Init");
    }
}