using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaveSystem : MonoBehaviour, IService
{
    public Camera Camera;
    private const float SpawnPadding = 1f;
    private bool _isSpawning;
    private int _aliveCount;

    private void Awake()
    {
        Camera = Camera.main;
    }

    public void Init()
    {
        Debug.Log($"[Wave] Initializing WaveSystem");
    }

    public async UniTask RunAllWaves()
    {
        var waves = CMS.GetAllData<TagWave>()
            .Select(x => x.tag)
            .OrderBy(w => w.Order)
            .ToList();

        Debug.Log($"[Wave] Loaded {waves.Count} waves");

        foreach (var wave in waves)
        {
            Debug.Log($"[Wave] Starting wave {wave.Order}");
            await RunWave(wave);
            await UniTask.WaitUntil(() => !_isSpawning && _aliveCount == 0);
            Debug.Log($"[Wave] Wave {wave.Order} cleared");
            await UniTask.Delay(2000);
        }

        Debug.Log("[Wave] All waves done");
    }

    public void NotifyEnemyKilled() => _aliveCount--;

    private async UniTask RunWave(TagWave wave)
    {
        _isSpawning = true;
        foreach (var entry in wave.Entries)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                SpawnEnemy(entry.EnemyPfb);
                _aliveCount++;
                await UniTask.Delay(entry.SpawnInterval);
            }
        }

        _isSpawning = false;
    }

    private void SpawnEnemy(CMSEntityPfb enemyPrefab)
    {
        var instance = Instantiate(enemyPrefab.gameObject, GetSpawnPoint(), Quaternion.identity);
        var enemy = instance.GetComponent<Enemy>();
        enemy.Init(enemyPrefab.AsEntity(), G.GameMain.player.transform);
    }

    private Vector2 GetSpawnPoint()
    {
        float halfH = Camera.orthographicSize + SpawnPadding;
        float halfW = halfH * Camera.aspect;
        Vector2 cp = Camera.transform.position;
        int side = Random.Range(0, 4);
        return side switch
        {
            0 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y + halfH),
            1 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y - halfH),
            2 => new Vector2(cp.x - halfW, Random.Range(cp.y - halfH, cp.y + halfH)),
            _ => new Vector2(cp.x + halfW, Random.Range(cp.y - halfH, cp.y + halfH))
        };
    }
}