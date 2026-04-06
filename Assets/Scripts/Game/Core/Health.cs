using Foundation.Events;
using UnityEngine;

namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 30;
        [SerializeField] private GameEvent onDiedEvent;

        private HealthModel _model;

        private void Awake()
        {
            _model = new HealthModel(maxHealth);
            _model.Died += OnDied;
        }

        private void OnDestroy()
        {
            _model.Died -= OnDied;
        }

        public void TakeDamage(int damage)
        {
            _model.TakeDamage(damage);
        }

        private void OnDied()
        {
            onDiedEvent?.Raise();
            Destroy(gameObject);
        }
    }
}
