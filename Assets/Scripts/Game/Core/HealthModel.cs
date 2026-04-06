using System;

namespace Game.Core
{
    public class HealthModel
    {
        public float Current { get; private set; }
        public float Max { get; }
        public bool IsDead => Current <= 0;

        public event Action Died;

        public HealthModel(float maxHealth)
        {
            Max = maxHealth;
            Current = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            Current -= damage;

            if (IsDead)
                Died?.Invoke();
        }
    }
}
