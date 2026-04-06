using UnityEngine;

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

        public Vector2 CalculateDirection(bool up, bool down, bool left, bool right)
        {
            var direction = Vector2.zero;
            if (up) direction += Vector2.up;
            if (down) direction += Vector2.down;
            if (left) direction += Vector2.left;
            if (right) direction += Vector2.right;
            direction.Normalize();
            return direction;
        }

        public bool TryShoot(float currentTime)
        {
            if (currentTime - _lastShotTime <= AttackCooldown)
                return false;

            _lastShotTime = currentTime;
            return true;
        }

        public Vector2 GetShootDirection(Vector2 shooterPos, Vector2 targetPos)
        {
            return (targetPos - shooterPos).normalized;
        }
    }
}
