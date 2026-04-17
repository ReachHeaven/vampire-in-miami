using Base;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Player
{
    public class PlayerView : ViewBase
    {
        public PlayerState State;
        private Vector2 _direction;
        private Rigidbody2D _rb;
        private Camera _camera;
        private float _lastShotTime;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _camera = Camera.main;

            var model = CMS.Get<CMSEntity>(Constants.Models.Player);
            State = new PlayerState(model);
            State.View = this;

            G.Player = this;
        }

        private void Update()
        {
            UpdateDirection();
            if (Mouse.current.leftButton.isPressed)
                TryShoot();
        }

        private void FixedUpdate()
        {
            MoveOnCollider();
        }

        private void MoveOnCollider()
        {
            float offset = 0.6f;
            Vector2 nextPos = _direction * (State.Speed * Time.fixedDeltaTime);

            if (G.Arena.collider.OverlapPoint((Vector2)transform.position + _direction * offset))
            {
                _rb.MovePosition(_rb.position + nextPos);
            }
            else
            {
                Vector2 smoothBack = Vector2.Lerp(_rb.position, _rb.position - nextPos, 0.01f);
                _rb.MovePosition(smoothBack);
            }
        }

        public void TakeDamage(int damage)
        {
            State.ApplyDamage(damage);
            G.Hud.SetHealth(State.Tag<TagStats>().MaxHealth, State.Health);

            if (State.IsDead)
                Destroy(gameObject);
        }
        

        private void TryShoot()
        {
            if (!State.Is<TagEquippedWeapon>(out var equipped)) return;
            var weapon = equipped.WeaponPfb.As<TagWeapon>();
            if (weapon == null || !weapon.bullet) return;
            if (Time.time - _lastShotTime < weapon.Cooldown) return;
            _lastShotTime = Time.time;

            Vector2 shooterPos = transform.position;
            var nearest = G.Waves.FindNearest(shooterPos, weapon.Range);
            Vector2 targetPos = nearest != null
                ? (Vector2)nearest.transform.position
                : (Vector2)_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (targetPos - shooterPos).normalized;

            Bullet.Spawn(weapon, shooterPos, direction);
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