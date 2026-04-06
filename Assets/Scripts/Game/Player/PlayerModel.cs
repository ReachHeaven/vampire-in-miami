namespace Game.Player
{
    public class PlayerModel
    {
        public float MoveSpeed { get; }
        public float AttackCooldown { get; }

        private float _lastShotTime;

        public PlayerModel(float moveSpeed, float attackCooldown)
        {
            MoveSpeed = moveSpeed;
            AttackCooldown = attackCooldown;
        }

        public bool TryShoot(float currentTime)
        {
            if (currentTime - _lastShotTime <= AttackCooldown)
                return false;

            _lastShotTime = currentTime;
            return true;
        }
    }
}
