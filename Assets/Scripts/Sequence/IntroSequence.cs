using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroSequence
{
    public async UniTask Play()
    {
        await Step1_VerifyCMS();
        await Step2_VerifyPlayer();
        await Step3_VerifyWeapon();
        await Step4_VerifyWaves();
        await Step5_RunWaves();
    }

    private async UniTask Step1_VerifyCMS()
    {
        G.HudView.SetMessage("Step 1: CMS");
        Debug.Log("[Test] Step 1: CMS contents");

        var entities = CMS.GetAll<CMSEntity>();
        Debug.Log($"  Total entities: {entities.Count}");
        foreach (var e in entities)
        {
            var tags = string.Join(", ", e.components.Select(c => c.GetType().Name));
            Debug.Log($"  - {e.id}: [{tags}]");
        }

        await UniTask.Delay(1000);
    }

    private async UniTask Step2_VerifyPlayer()
    {
        G.HudView.SetMessage("Step 2: Player");
        Debug.Log("[Test] Step 2: Player");

        if (G.GameMain.player == null)
        {
            Debug.LogError("  Player NULL!");
            return;
        }

        var stats = G.GameMain.player.Stats;
        Debug.Log($"  HP: {stats.Health}/{stats.MaxHealth}, Speed: {stats.Speed}");

        Debug.Log("  Damage 25...");
        G.GameMain.player.TakeDamage(25);
        Debug.Log($"  After: {stats.Health}/{stats.MaxHealth}");

        Debug.Log("  Heal 25...");
        G.HealthSystem.Heal(G.GameMain.player.Instance, 25);
        Debug.Log($"  After: {stats.Health}/{stats.MaxHealth}");

        await UniTask.Delay(1000);
    }

    private async UniTask Step3_VerifyWeapon()
    {
        G.HudView.SetMessage("Step 3: Weapon");
        Debug.Log("[Test] Step 3: Weapon check");

        var playerEntity = G.GameMain.player.Instance;

        if (!playerEntity.Is<TagWeapon>(out var weapon))
        {
            Debug.LogError("  Player has no TagWeapon!");
            return;
        }

        Debug.Log($"  Weapon: dmg={weapon.Damage}, cooldown={weapon.Cooldown}, range={weapon.Range}");

        if (!weapon.bullet)
        {
            Debug.LogError("  bulletPrefab is NULL!");
            return;
        }

        var bulletEntity = weapon.bullet.AsEntity();
        var bulletStats = bulletEntity.Get<TagBullet>();
        // Debug.Log($"  Bullet: speed={bulletStats.Speed}, dmg={bulletStats.Damage}");
        // Debug.Log($"  Bullet prefab: {weapon.bullet.gameObject.name}");

        await UniTask.Delay(1000);
    }

    private async UniTask Step4_VerifyWaves()
    {
        G.HudView.SetMessage("Step 4: Waves");
        Debug.Log("[Test] Step 4: Wave definitions");

        var waves = CMS.GetAllData<TagWave>()
            .Select(x => x.tag)
            .OrderBy(w => w.Order)
            .ToList();

        Debug.Log($"  Found {waves.Count} waves");
        foreach (var wave in waves)
        {
            int total = wave.Entries.Sum(e => e.Count);
            Debug.Log($"  - order={wave.Order}, enemies={total}");
            foreach (var entry in wave.Entries)
            {
                var name = entry.EnemyPfb != null ? entry.EnemyPfb.name : "NULL";
                Debug.Log($"    {entry.Count}x {name} every {entry.SpawnInterval}ms");
            }
        }

        await UniTask.Delay(1000);
    }

    private async UniTask Step5_RunWaves()
    {
        G.HudView.SetMessage("Step 5: Fight!");
        Debug.Log("[Test] Step 5: Running waves");

        await G.WaveSystem.RunAllWaves();

        G.HudView.SetMessage("Victory!");
        Debug.Log("[Test] All done");
    }
}