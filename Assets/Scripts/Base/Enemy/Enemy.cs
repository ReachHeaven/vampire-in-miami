using System;
using Base.Interfaces;
using UnityEngine;

namespace Base.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public EnemyData EnemyData;
        private Health _health;
        private Transform _target;
        private Rigidbody2D _rb;
        public Action OnDeath;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }

        public void Init(Transform target)
        {
            _health = new Health(EnemyData.MaxHealth);
            _target = target;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        public void Die()
        {
            if (_health.isDead())
            {
                Destroy(gameObject);
                OnDeath.Invoke();
            }
        }

        private void MoveToTarget()
        {
            if (_target) return;

            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (EnemyData.Speed * Time.fixedDeltaTime));
        }
    }
}