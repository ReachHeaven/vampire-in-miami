using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private float _speed = 10f;
        private Transform _target;
        private Rigidbody2D _rb;


        public void Init(Transform target, float speed)
        {
            _target = target;
            _speed = speed;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_target) return;
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (_speed * Time.fixedDeltaTime));
        }
    }
}
