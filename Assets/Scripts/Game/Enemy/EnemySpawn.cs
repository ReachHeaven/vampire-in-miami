using Game.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Waves
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float spawnPadding = 1f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public EnemyAdapter Spawn(EnemyAdapter prefab)
        {
            EnemyAdapter instance = Instantiate(prefab, GetSpawnPoint(), Quaternion.identity);
            instance.Init(playerTransform);
            return instance;
        }

        private Vector2 GetSpawnPoint()
        {
            float halfHeight = _camera.orthographicSize + spawnPadding;
            float halfWidth = halfHeight * _camera.aspect;

            Vector2 cameraPosition = _camera.transform.position;
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
    }
}