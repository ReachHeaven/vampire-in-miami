using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Player
{
    public class Player : MonoBehaviour
    {
        public TagStats Stats;
        public CMSEntity Instance;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            Instance = CMS.Get<CMSEntity>(Constants.Models.Player).DeepCopy();
            Stats = Instance.Get<TagStats>();
            Stats.Health = Stats.MaxHealth;
        }

        private void Update()
        {
            UpdateDirection();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                G.FightSystem.Shoot(Instance, this);
            }
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (Stats.Speed * Time.fixedDeltaTime));
        }


        public void TakeDamage(int damage)
        {
            G.HealthSystem.ApplyDamage(Instance, damage, this);
        }

        private void UpdateDirection()
        {
            Keyboard kb = Keyboard.current;
            _direction = Vector2.zero;
            if (kb.upArrowKey.isPressed || kb.wKey.isPressed) _direction += Vector2.up;
            if (kb.downArrowKey.isPressed || kb.sKey.isPressed) _direction += Vector2.down;
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed) _direction += Vector2.left;
            if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) _direction += Vector2.right;
            _direction.Normalize();
        }
    }
}