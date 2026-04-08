using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Base
{
    public class Health
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public bool isDead => CurrentHealth <= 0;

        public event Action OnDied;

        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;
            CurrentHealth -= damage;
            if (isDead) OnDied?.Invoke();
        }
    }
}