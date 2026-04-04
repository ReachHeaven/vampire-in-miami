using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private Vector2 _direction = Vector2.zero;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            _direction = Vector2.zero;
            
            if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed) _direction += Vector2.up;
            if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed) _direction += Vector2.down;
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) _direction += Vector2.left;
            if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) _direction += Vector2.right;
            _direction.Normalize();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}