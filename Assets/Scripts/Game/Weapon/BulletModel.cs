    using UnityEngine;

namespace Game.Weapon
{
    public class BulletModel
    {
        public float Speed { get; }
        public int Damage { get; }
        public Vector2 Direction { get; private set; }

        public BulletModel(float speed, int damage)
        {
            Speed = speed;
            Damage = damage;
        }

        public void Init(Vector2 direction)
        {
            Direction = direction;
        }
    }
}
