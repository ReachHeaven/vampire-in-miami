using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroSequence
{
    public async UniTask Play()
    {
        await Step1_VerifyCMS();
        await Step2_VerifyPlayer();
        await Step3_VerifyWaves();
        await Step4_RunFirstWave();
    }

    private async UniTask Step1_VerifyCMS()
    {
        G.HudView.SetMessage("Step 1: CMS verification");
        Debug.Log("[Test] Step 1: CMS contents");

        var entities = CMS.GetAll<CMSEntity>();
        Debug.Log($"  Total CMS entities: {entities.Count}");
        foreach (var e in entities)
            Debug.Log($"  - {e.id} ({e.components.Count} components)");

        await UniTask.Delay(2000);
    }

    private async UniTask Step2_VerifyPlayer()
    {
        G.HudView.SetMessage("Step 2: Player check");
        Debug.Log("[Test] Step 2: Player");

        if (G.GameMain.player == null)
        {
            Debug.LogError("  Player is NULL in scene!");
            return;
        }

        var stats = G.GameMain.player.Stats;
        Debug.Log($"  Player HP: {stats.Health}/{stats.MaxHealth}, Speed: {stats.Speed}");

        Debug.Log("  Applying 25 test damage...");
        G.GameMain.player.TakeDamage(25);
        Debug.Log($"  After damage: {stats.Health}/{stats.MaxHealth}");

        Debug.Log("  Healing 10...");
        G.HealthSystem.Heal(G.GameMain.player.Instance, 10);
        Debug.Log($"  After heal: {stats.Health}/{stats.MaxHealth}");

        await UniTask.Delay(2000);
    }

    private async UniTask Step3_VerifyWaves()
    {
        G.HudView.SetMessage("Step 3: Wave definitions");
        Debug.Log("[Test] Step 3: Waves in CMS");

        var waves = CMS.GetAllData<TagWave>()
            .Select(x => x.tag)
            .OrderBy(w => w.Order)
            .ToList();

        Debug.Log($"  Found {waves.Count} wave definitions");
        foreach (var wave in waves)
        {
            int totalEnemies = wave.Entries.Sum(e => e.Count);
            Debug.Log($"  - Wave order={wave.Order}, total enemies={totalEnemies}");
            foreach (var entry in wave.Entries)
            {
                var enemyName = entry.EnemyPfb != null ? entry.EnemyPfb.name : "NULL";
                Debug.Log($"    · {entry.Count}× {enemyName} every {entry.SpawnInterval}ms");
            }
        }

        await UniTask.Delay(2000);
    }

    private async UniTask Step4_RunFirstWave()
    {
        G.HudView.SetMessage("Step 4: Running waves");
        Debug.Log("[Test] Step 4: Running waves");

        await G.WaveSystem.RunAllWaves();

        G.HudView.SetMessage("All waves cleared");
        Debug.Log("[Test] All done");
    }
}