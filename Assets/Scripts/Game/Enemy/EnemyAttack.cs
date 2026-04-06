using System;
using Game.Core;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField]  private int damage = 30;
        [SerializeField] private float attackInterval = 1f;
        private float _lastAttackTime;

        private void OnCollisionStay2D(Collision2D other)
        {
            _lastAttackTime += Time.deltaTime;
            if (_lastAttackTime < attackInterval) return;
            if (!other.collider.TryGetComponent(out Health health)) return;
            health.TakeDamage(damage);
            _lastAttackTime = 0f;
        }
    }
}
