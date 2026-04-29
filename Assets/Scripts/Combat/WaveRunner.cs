using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    private Camera _camera;
    private bool _isSpawning;
    private readonly List<EnemyView> _aliveEnemies = new();

    private void Awake()
    {
        _camera = Camera.main;
        G.Waves = this;
    }

    public async UniTask RunAll()
    {
        var tag = CMS.GetAllData<TagAllWaves>()
            .Select(x => x.tag)
            .FirstOrDefault();

        if (tag == null) return;

        foreach (var wave in tag.Waves.OrderBy(w => w.Order))
        {
            await RunWave(wave);
            await UniTask.WaitUntil(() => !_isSpawning && _aliveEnemies.Count == 0);
            await UniTask.Delay(2000);
        }

        G.GameMain.OnAllWavesCleared();
    }

    public void NotifyKilled(EnemyView enemy)
    {
        _aliveEnemies.Remove(enemy);
    }

    public EnemyView FindNearest(Vector2 origin, float radius)
    {
        EnemyView nearest = null;
        float minSqr = radius * radius;

        foreach (var enemy in _aliveEnemies)
        {
            if (!enemy) continue;
            float sqr = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = enemy;
            }
        }

        return nearest;
    }

    private async UniTask RunWave(WaveDefinition wave)
    {
        G.Hud.SetMessage($"Wave {wave.Order}");
        _isSpawning = true;

        var spawnPool = new List<(CMSEntity model, int interval)>();

        foreach (var entry in wave.Entries)
        {
            for (int i = 0; i < entry.Count; i++)
                spawnPool.Add((entry.EnemyPfb.AsEntity(), entry.SpawnInterval));
        }

        // Fisher-Yates shuffle (uniform; OrderBy(Random.value) is biased)
        for (int i = spawnPool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnPool[i], spawnPool[j]) = (spawnPool[j], spawnPool[i]);
        }

        foreach (var spawnData in spawnPool)
        {
            SpawnEnemy(spawnData.model);
            await UniTask.Delay(spawnData.interval);
        }

        _isSpawning = false;
    }

    private void SpawnEnemy(CMSEntity model)
    {
        var enemy = G.EnemyFactory.Create(model);
        enemy.transform.position = MathUtil.GetSpawnPoint(_camera);
        enemy.SetTarget(G.Player.transform);
        _aliveEnemies.Add(enemy);
    }

    public void KillAllEnemies()
    {
        foreach (var e in _aliveEnemies)
            if (e)
                Destroy(e.gameObject);

        _aliveEnemies.Clear();
    }
}