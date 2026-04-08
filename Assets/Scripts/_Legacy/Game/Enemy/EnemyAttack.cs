using System;
using Game.Core;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private int _damage = 30;
        private float _attackInterval = 1f;
        private float _lastAttackTime;

        public void Init(int damage, float attackInterval)
        {
            _damage = damage;
            _attackInterval = attackInterval;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            _lastAttackTime += Time.deltaTime;
            if (_lastAttackTime < _attackInterval) return;
            if (!other.collider.TryGetComponent(out Health health)) return;
            health.TakeDamage(_damage);
            _lastAttackTime = 0f;
        }
    }
}
