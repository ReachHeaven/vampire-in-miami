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
        [SerializeField] private Transform _visual;
        private Vector2 _direction;
        private Rigidbody2D _rb;
        private Camera _camera;

        [FormerlySerializedAs("_playerAnimation")]
        public PlayerAnimation PlayerAnimation;

        private float _lastShotTime;
        private Vector2 _lastFixedPos;

        private Transform Visual => _visual != null ? _visual : transform;

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
            if (_direction.x != 0)
            {
                float scaleX = _direction.x < 0 ? -1f : 1f;
                var v = Visual;
                var s = v.localScale;
                s.x = scaleX;
                v.localScale = s;
            }

            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.isPressed)
                TryShoot();
        }

        private void FixedUpdate()
        {
            MoveOnCollider();
        }

        private void MoveOnCollider()
        {
            Vector2 actualDelta = _rb.position - _lastFixedPos;
            bool actuallyMoved = actualDelta.sqrMagnitude > 0.0001f;
            _lastFixedPos = _rb.position;

            Vector2 targetPosition = _rb.position + _direction * (State.Speed * Time.fixedDeltaTime);
            _rb.MovePosition(targetPosition);

            if (actuallyMoved && _direction.sqrMagnitude > 0.01f && !DOTween.IsTweening(Visual))
            {
                Visual.DOScaleY(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
            }
        }

        public void TakeDamage(int damage)
        {
            State.ApplyDamage(damage);
            G.Hud.SetHealth(State.MaxHealth, State.Health);

            if (State.IsDead)
            {
                G.Player = null;
                Destroy(gameObject);
            }
        }

        private void TryShoot()
        {
            if (!State.HasWeapon) return;
            var w = State.Weapon;
            if (Time.time - _lastShotTime < w.Cooldown) return;
            _lastShotTime = Time.time;

            Vector2 shooterPos = transform.position;
            var nearest = G.Waves.FindNearest(shooterPos, w.Range);
            Vector2 targetPos;
            if (nearest != null)
            {
                targetPos = nearest.transform.position;
            }
            else
            {
                var mouse = Mouse.current;
                if (mouse == null) return;
                targetPos = _camera.ScreenToWorldPoint(mouse.position.ReadValue());
            }

            Vector2 direction = (targetPos - shooterPos).normalized;

            _visual.DOShakePosition(0.1f, strength: 0.5f, vibrato: 0);
            Bullet.Spawn(w.BulletPfb, shooterPos, direction, w.Damage, w.BulletSpeed);
        }

        private void UpdateDirection()
        {
            _direction = Vector2.zero;
            Keyboard kb = Keyboard.current;
            if (kb == null) return;
            if (kb.upArrowKey.isPressed || kb.wKey.isPressed) _direction += Vector2.up;
            if (kb.downArrowKey.isPressed || kb.sKey.isPressed) _direction += Vector2.down;
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed) _direction += Vector2.left;
            if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) _direction += Vector2.right;
            _direction.Normalize();
        }
    }
}