namespace Game.Enemy
{
    public class EnemyModel
    {
        public float Speed { get; }
        public int Damage { get; }
        public float AttackInterval { get; }

        private float _attackTimer;

        public EnemyModel(float speed, int damage, float attackInterval)
        {
            Speed = speed;
            Damage = damage;
            AttackInterval = attackInterval;
        }

        public bool TryAttack(float deltaTime)
        {
            _attackTimer += deltaTime;
            if (_attackTimer < AttackInterval)
                return false;

            _attackTimer = 0f;
            return true;
        }
    }
}
