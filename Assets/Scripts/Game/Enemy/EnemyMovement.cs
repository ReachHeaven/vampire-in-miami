using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private Transform _target;
        private Rigidbody2D _rb;


        public void Init(Transform target)
        {
            _target = target;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_target) return;
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (speed * Time.fixedDeltaTime));
        }
    }
}