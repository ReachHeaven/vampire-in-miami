using Foundation.Events;
using UnityEngine;

namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 30;
        [SerializeField] private GameEvent onDiedEvent;
        private float _currentHealth;


        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (!IsDead()) return;
            onDiedEvent?.Raise();
            Dead();
        }

        private void Dead()
        {
            Destroy(gameObject);
        }

        private bool IsDead()
        {
            return _currentHealth <= 0;
        }
    }
}