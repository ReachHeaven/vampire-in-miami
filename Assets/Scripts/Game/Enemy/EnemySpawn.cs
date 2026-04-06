using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private EnemyMovement enemy;
        [SerializeField] private float spawnInterval;
        [SerializeField] private float spawnPadding = 1f;
        [SerializeField] private Transform playerTransform;

        private Camera _camera;
        private EnemySpawnModel _model;

        private void Awake()
        {
            _camera = Camera.main;
            _model = new EnemySpawnModel(spawnInterval);
        }

        private void Update()
        {
            if (_model.ShouldSpawn(Time.deltaTime))
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector2 spawnPoint = _model.GetSpawnPoint(
                _camera.transform.position,
                _camera.orthographicSize,
                _camera.aspect,
                spawnPadding);

            EnemyMovement instance = Instantiate(enemy, spawnPoint, Quaternion.identity);
            instance.Init(playerTransform);
        }
    }
}
