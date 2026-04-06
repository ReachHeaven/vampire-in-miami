using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawnModel
    {
        private readonly float _spawnInterval;
        private float _timer;

        public EnemySpawnModel(float spawnInterval)
        {
            _spawnInterval = spawnInterval;
        }

        public bool ShouldSpawn(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < _spawnInterval)
                return false;

            _timer = 0f;
            return true;
        }

        public Vector2 GetSpawnPoint(Vector2 cameraPos, float orthoSize, float aspect, float padding)
        {
            var halfHeight = orthoSize + padding;
            var halfWidth = halfHeight * aspect;

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
