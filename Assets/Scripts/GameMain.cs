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
        var test = CMS.Get<CMSTest>();
        Debug.Log($"CMSTest: {CMS.Get<CMSTest>()}");
        var copy = test.DeepCopy();

        var testHealth = test.Get<TagHealth>();
        var copyHealth = copy.Get<TagHealth>();

        Debug.Log($"Same list? {ReferenceEquals(test.components, copy.components)}");
        Debug.Log($"Same TagHealth? {ReferenceEquals(testHealth, copyHealth)}");
        Debug.Log($"test count: {test.components.Count}, copy count: {copy.components.Count}");

        copyHealth.Current = 150;
        Debug.Log($"Original: {testHealth.Current}, Copy: {copyHealth.Current}");
    }
}