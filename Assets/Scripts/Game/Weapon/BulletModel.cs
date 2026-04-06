namespace Game.Weapon
{
    public class BulletModel
    {
        public float Speed { get; }
        public int Damage { get; }

        public BulletModel(float speed, int damage)
        {
            Speed = speed;
            Damage = damage;
        }
    }
}
