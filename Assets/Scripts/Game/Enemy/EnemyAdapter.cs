using Game.Core;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack), typeof(Health))]
    public class EnemyAdapter : MonoBehaviour
    {
        [SerializeField] private EnemyConfig config;
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private EnemyAttack attack;
        [SerializeField] private Health health;

        private void Reset()
        {
            movement = GetComponent<EnemyMovement>();
            attack = GetComponent<EnemyAttack>();
            health = GetComponent<Health>();
        }

        public void Init(Transform target)
        {
            movement.Init(target, config.Speed);
            attack.Init(config.Damage, config.AttackInterval);
            health.Init(config.Health);
        }
    }
}