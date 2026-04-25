using Base;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Base.Player
{
    public class PlayerView : ViewBase
    {
        public PlayerState State;
        private Vector2 _direction;
        private Rigidbody2D _rb;
        private Camera _camera;
        [FormerlySerializedAs("_playerAnimation")] public PlayerAnimation PlayerAnimation;
        private float _lastShotTime;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            PlayerAnimation = GetComponent<PlayerAnimation>();
            _camera = Camera.main;

            var model = CMS.Get<CMSEntity>(Constants.Models.Player);
            State = new PlayerState(model);
            State.View = this;

            G.Player = this;
        }

        private void Update()
        {
            UpdateDirection();
            // PlayerAnimation.SetMoving(_direction.magnitude > 0.01f);
            if (_direction.x != 0)
            {
                float scaleX = _direction.x < 0 ? -1f : 1f;
                transform.localScale = new Vector3(scaleX, 1f, 1f);
            }
            if (Mouse.current.leftButton.isPressed)
                TryShoot();
        }

        private void FixedUpdate()
        {
            MoveOnCollider();
        }

        private void MoveOnCollider()
        {
            Vector2 targetPosition = _rb.position + _direction * (State.Speed * Time.fixedDeltaTime);
            _rb.MovePosition(targetPosition);

            if (_direction.sqrMagnitude > 0.01f && !DOTween.IsTweening(transform))
            {
                transform.DOScaleY(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
            }
        }

        public void TakeDamage(int damage)
        {
            State.ApplyDamage(damage);
            G.Hud.SetHealth(State.MaxHealth, State.Health);

            if (State.IsDead)
                Destroy(gameObject);
        }

        private void TryShoot()
        {
            if (!State.HasWeapon) return;
            var w = State.Weapon;
            if (Time.time - _lastShotTime < w.Cooldown) return;
            _lastShotTime = Time.time;

            Vector2 shooterPos = transform.position;
            var nearest = G.Waves.FindNearest(shooterPos, w.Range);
            Vector2 targetPos = nearest != null
                ? (Vector2)nearest.transform.position
                : (Vector2)_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (targetPos - shooterPos).normalized;

            Bullet.Spawn(w.BulletPfb, shooterPos, direction, w.Damage, w.BulletSpeed);
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