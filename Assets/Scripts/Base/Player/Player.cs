using System;
using Base.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public PlayerData PlayerData;
        private Health _health;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        public Health Health => _health;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _health = new Health(PlayerData.MaxHealth);
            _health.OnDied += HandleDied;
        }

        private void Update()
        {
            UpdateDirection();
        }

        private void FixedUpdate()
        {
            ChaseTarget();
        }

        private void ChaseTarget()
        {
            _rb.MovePosition(_rb.position + _direction * (PlayerData.Speed * Time.fixedDeltaTime));
        }

        private void UpdateDirection()
        {
            Keyboard keyboard = Keyboard.current;
            _direction = Vector2.zero;

            if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed) _direction += Vector2.up;
            if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed) _direction += Vector2.down;
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) _direction += Vector2.left;
            if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) _direction += Vector2.right;
            _direction.Normalize();
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        private void HandleDied()
        {
            _health.OnDied -= HandleDied;
            Destroy(gameObject);
        }
    }
}