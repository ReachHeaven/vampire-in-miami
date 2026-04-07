using System.Collections;
using Game.Core;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Waves
{
    namespace Game.Waves
    {
        public class WaveSpawner : MonoBehaviour
        {
            [SerializeField] private EnemySpawner enemySpawner;
            [SerializeField] private WavesConfig[] waves;
            [SerializeField] private float delayBeforeFirstWave = 2f;
            [SerializeField] private float delayBetweenWaves = 3f;

            private int _aliveCount;

            private void Start()
            {
                StartCoroutine(RunWaves());
            }

            private IEnumerator RunWaves()
            {
                yield return new WaitForSeconds(delayBeforeFirstWave);

                for (int i = 0; i < waves.Length; i++)
                {
                    yield return RunWave(waves[i]);

                    if (i < waves.Length - 1)
                        yield return new WaitForSeconds(delayBetweenWaves);
                }
            }

            private IEnumerator RunWave(WavesConfig wave)
            {
                _aliveCount = 0;

                foreach (var entry in wave.Entries)
                {
                    for (var i = 0; i < entry.Count; i++)
                    {
                        EnemyAdapter enemy = enemySpawner.Spawn(entry.EnemyConfig);
                        enemy.TryGetComponent<Health>(out  Health health);
                        _aliveCount++;
                    }
                }

                while (_aliveCount > 0)
                    yield return null;
            }

            private void HandleEnemyDied()
            {
                _aliveCount--;
            }
        }
    }
}