using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 10f;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (speed * Time.fixedDeltaTime));
        }
    }
}