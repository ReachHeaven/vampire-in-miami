using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] EnemyMovement enemy;
        [SerializeField] float spawnInterval;
        [SerializeField] float spawnPadding = 1f;
        [SerializeField] Transform playerTransform;

        private Camera _camera;
        private float _timer;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                _timer = 0f;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            EnemyMovement instance = Instantiate(enemy, GetSpawnPoint(), Quaternion.identity);
            instance.Init(playerTransform);
        }

        private Vector2 GetSpawnPoint()
        {
            var halfHeight = _camera.orthographicSize + spawnPadding;
            var halfWidth = halfHeight * _camera.aspect;

            Vector2 cameraPostion = _camera.transform.position;
            int randomSide = Random.Range(0, 4);

            return randomSide switch
            {
                0 => new Vector2(Random.Range(cameraPostion.x - halfWidth, cameraPostion.x + halfWidth),
                    cameraPostion.y + halfHeight),
                1 => new Vector2(Random.Range(cameraPostion.x - halfWidth, cameraPostion.x + halfWidth),
                    cameraPostion.y - halfHeight),
                2 => new Vector2(cameraPostion.x - halfWidth,
                    Random.Range(cameraPostion.y - halfHeight, cameraPostion.y + halfHeight)),
                _ => new Vector2(cameraPostion.x + halfWidth,
                    Random.Range(cameraPostion.y - halfHeight, cameraPostion.y + halfHeight))
            };
        }
    }
}