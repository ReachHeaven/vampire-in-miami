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
    }
}
