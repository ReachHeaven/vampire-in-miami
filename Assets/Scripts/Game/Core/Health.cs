using Foundation.Events;
using UnityEngine;

namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        private float _maxHealth = 30;
        [SerializeField] private GameEvent onDiedEvent;
        private float _currentHealth;
        private bool IsDead() => _currentHealth <= 0;

        public void Init(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
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
    }
}