using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackCooldown = 1f;

        public PlayerModel Model { get; private set; }

        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            Model = new PlayerModel(moveSpeed, attackCooldown);
        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            Keyboard keyboard = Keyboard.current;

            bool up = keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed;
            bool down = keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed;
            bool left = keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed;
            bool right = keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed;

            _direction = Vector2.zero;
            if (up) _direction += Vector2.up;
            if (down) _direction += Vector2.down;
            if (left) _direction += Vector2.left;
            if (right) _direction += Vector2.right;
            _direction.Normalize();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (Model.MoveSpeed * Time.fixedDeltaTime));
        }
    }
}
