using System.Linq;
using Base.Player;
using Cysharp.Threading.Tasks;
using Test;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public Player player;

    public async void Init()
    {
        player = FindFirstObjectByType<Player>();
        
        Debug.Log("=== GameMain.Init ===");

        await new IntroSequence().Play();
    }
}