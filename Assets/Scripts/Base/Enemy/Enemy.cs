using System;
using Base.Interfaces;
using UnityEngine;

namespace Base.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public EnemyData EnemyData;
        public Health Health;
        private Transform _target;
        private Rigidbody2D _rb;

        private void Awake()
        {
            Health.OnDied += Die;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }

        public void Init(Transform target)
        {
            Health = new Health(EnemyData.MaxHealth);
            _target = target;
        }

        public void TakeDamage(int damage)
        {
            Health.TakeDamage(damage);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private void MoveToTarget()
        {
            if (!_target) return;

            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (EnemyData.Speed * Time.fixedDeltaTime));
        }
    }
}