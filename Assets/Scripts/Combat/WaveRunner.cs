using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    private Camera _camera;
    private bool _isSpawning;
    private int _aliveCount;
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
            Debug.Log($"Run wave {wave.Order}");
            await RunWave(wave);
            await UniTask.WaitUntil(() => !_isSpawning && _aliveCount == 0);
            await UniTask.Delay(2000);
        }

        G.GameMain.OnAllWavesCleared();
    }

    public void NotifyKilled(EnemyView enemy)
    {
        _aliveEnemies.Remove(enemy);
        _aliveCount--;
        Debug.Log($"NotifyKilled, aliveCount {_aliveCount}");
    }

    public EnemyView FindNearest(Vector2 origin, float radius)
    {
        EnemyView nearest = null;
        float minDist = radius;

        foreach (var enemy in _aliveEnemies)
        {
            if (!enemy) continue;
            float dist = Vector2.Distance(origin, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
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

        var shuffledPool = spawnPool.OrderBy(x => UnityEngine.Random.value).ToList();

        foreach (var spawnData in shuffledPool)
        {
            SpawnEnemy(spawnData.model);
            _aliveCount++;
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
        _aliveCount = 0;
    }
}