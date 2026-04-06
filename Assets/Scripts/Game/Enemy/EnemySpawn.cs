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
            Vector2 spawnPoint = GetSpawnPoint();
            EnemyMovement instance = Instantiate(enemy, spawnPoint, Quaternion.identity);
            instance.Init(playerTransform);
        }

        private Vector2 GetSpawnPoint()
        {
            var halfHeight = _camera.orthographicSize + spawnPadding;
            var halfWidth = halfHeight * _camera.aspect;

            Vector2 cameraPos = _camera.transform.position;
            int randomSide = Random.Range(0, 4);

            return randomSide switch
            {
                0 => new Vector2(Random.Range(cameraPos.x - halfWidth, cameraPos.x + halfWidth),
                    cameraPos.y + halfHeight),
                1 => new Vector2(Random.Range(cameraPos.x - halfWidth, cameraPos.x + halfWidth),
                    cameraPos.y - halfHeight),
                2 => new Vector2(cameraPos.x - halfWidth,
                    Random.Range(cameraPos.y - halfHeight, cameraPos.y + halfHeight)),
                _ => new Vector2(cameraPos.x + halfWidth,
                    Random.Range(cameraPos.y - halfHeight, cameraPos.y + halfHeight))
            };
        }
    }
}
