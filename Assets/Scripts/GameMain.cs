using Base.Player;
using Cysharp.Threading.Tasks;
using Foundation.CMS;
using Tags;
using Test;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public Player player;

    public async UniTask Init()
    {
        foreach (var (health, speed, entity) in CMS.QueryWithEntity<TagHealth, TagSpeed>())
        {
            Debug.Log($"ID:  {entity.id}: hp={health.Current}, speed={speed.Value}");
        }
        // CMSTest.RunCmsTests();RunCmsTests

        // await new IntroSequence().Play();
    }
}