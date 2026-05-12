using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    private Camera _camera;
    private bool _isSpawning;
    private readonly List<EnemyView> _aliveEnemies = new();
    private Bounds _arenaInner;
    private bool _hasArenaBounds;

    private void Awake()
    {
        _camera = Camera.main;
        G.Waves = this;
        ComputeArenaBounds();
    }

    private void ComputeArenaBounds()
    {
        var walls = GameObject.Find("walls");
        if (walls == null) return;

        var colliders = walls.GetComponentsInChildren<Collider2D>();
        if (colliders.Length == 0) return;

        Bounds outer = colliders[0].bounds;
        for (int i = 1; i < colliders.Length; i++)
            outer.Encapsulate(colliders[i].bounds);

        float xMin = outer.min.x;
        float xMax = outer.max.x;
        float yMin = outer.min.y;
        float yMax = outer.max.y;

        foreach (var c in colliders)
        {
            var b = c.bounds;
            if (b.size.x > b.size.y)
            {
                if (b.center.y > outer.center.y) yMax = Mathf.Min(yMax, b.min.y);
                else yMin = Mathf.Max(yMin, b.max.y);
            }
            else
            {
                if (b.center.x > outer.center.x) xMax = Mathf.Min(xMax, b.min.x);
                else xMin = Mathf.Max(xMin, b.max.x);
            }
        }

        _arenaInner = new Bounds(
            new Vector3((xMin + xMax) * 0.5f, (yMin + yMax) * 0.5f),
            new Vector3(xMax - xMin, yMax - yMin));
        _hasArenaBounds = true;
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
        enemy.transform.position = GetSpawnPointInsideArena();
        enemy.SetTarget(G.Player.transform);
        _aliveEnemies.Add(enemy);
    }

    private Vector2 GetSpawnPointInsideArena()
    {
        const float cameraPad = 0.5f;
        const float wallPad = 0.5f;

        float halfH = _camera.orthographicSize + cameraPad;
        float halfW = halfH * _camera.aspect;
        Vector2 cp = _camera.transform.position;

        float top = cp.y + halfH;
        float bottom = cp.y - halfH;
        float left = cp.x - halfW;
        float right = cp.x + halfW;

        if (_hasArenaBounds)
        {
            top = Mathf.Min(top, _arenaInner.max.y - wallPad);
            bottom = Mathf.Max(bottom, _arenaInner.min.y + wallPad);
            left = Mathf.Max(left, _arenaInner.min.x + wallPad);
            right = Mathf.Min(right, _arenaInner.max.x - wallPad);
        }

        int side = Random.Range(0, 4);
        return side switch
        {
            0 => new Vector2(Random.Range(left, right), top),
            1 => new Vector2(Random.Range(left, right), bottom),
            2 => new Vector2(left, Random.Range(bottom, top)),
            _ => new Vector2(right, Random.Range(bottom, top))
        };
    }

    public void KillAllEnemies()
    {
        foreach (var e in _aliveEnemies)
            if (e)
                Destroy(e.gameObject);

        _aliveEnemies.Clear();
    }
}