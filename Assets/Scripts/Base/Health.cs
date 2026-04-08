using System;
using UnityEngine;

namespace Base
{
    public class Health
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public bool isDead() => CurrentHealth >= 0;

        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (isDead())
            {
                Debug.Log($"Health is {CurrentHealth} / {MaxHealth}");
            }
        }
    }
}