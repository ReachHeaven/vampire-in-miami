using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerData PlayerData;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
    }
}