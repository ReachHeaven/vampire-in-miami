using System;
using System.Collections;
using Base.Enemy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Base.Wave
{
    public class WavePhase : MonoBehaviour
    {
        public WaveData[] waves;
        public Camera Camera;
        private const float _spawnPadding = 1f;
        private int waveCount;
        private bool _isSpawning;
        private int _aliveCount;

        private void Awake()
        {
            Camera = Camera.main;
        }

        private async UniTask RunWaves()
        {
            foreach (var wave in waves)
            {
                await RunWave(wave);
                await UniTask.WaitUntil(this, t => t._aliveCount > 0 && !_isSpawning);
                await UniTask.Delay(100);
            }
        }

        private async UniTask RunWave(WaveData wave)
        {
            _isSpawning = true;
            foreach (var enemy in wave.waveEnemies)
            {
                for (int i = 0; i < enemy.count; i++)
                {
                    SpawnEnemy(enemy.Enemy);
                    _aliveCount++;
                    await UniTask.Delay(enemy.spawnInterval);
                }
            }
            _isSpawning = false;
        }

        private void SpawnEnemy(EnemyData data)
        {
            var enemy = Instantiate(data.EnemyPrefab, GetSpawnPoint(), Quaternion.identity);
            enemy.OnDeath += OnEnemyDied;
            enemy.Init(G.GameMain.player.transform);
        }

        private Vector2 GetSpawnPoint()
        {
            float halfHeight = Camera.orthographicSize + _spawnPadding;
            float halfWidth = halfHeight * Camera.aspect;

            Vector2 cameraPosition = Camera.transform.position;
            int randomSide = Random.Range(0, 4);

            return randomSide switch
            {
                0 => new Vector2(Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth),
                    cameraPosition.y + halfHeight),
                1 => new Vector2(Random.Range(cameraPosition.x - halfWidth, cameraPosition.x + halfWidth),
                    cameraPosition.y - halfHeight),
                2 => new Vector2(cameraPosition.x - halfWidth,
                    Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight)),
                _ => new Vector2(cameraPosition.x + halfWidth,
                    Random.Range(cameraPosition.y - halfHeight, cameraPosition.y + halfHeight))
            };
        }

        private void OnEnemyDied()
        {
            _aliveCount--;
        }
    }
}