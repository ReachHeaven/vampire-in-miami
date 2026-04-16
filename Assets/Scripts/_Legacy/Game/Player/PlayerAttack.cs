using System.Collections.Generic;
using Game.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float cooldown = 1;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float aimRadius = 10f;
        [SerializeField] private LayerMask enemyLayer;

        private static readonly List<Collider2D> HitBuffer = new();
        private ContactFilter2D _contactFilter;
        private float _lastShotTime;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _contactFilter = new ContactFilter2D();
            _contactFilter.SetLayerMask(enemyLayer);
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && Time.time - _lastShotTime > cooldown)
            {
                Shoot();
                _lastShotTime = Time.time;
            }
        }

        private void Shoot()
        {
            Vector2 shooterPos = transform.position;
            Vector2 targetPos = FindNearestEnemyPosition(shooterPos) ?? GetMouseWorldPosition();
            Vector2 direction = (targetPos - shooterPos).normalized;

            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // bullet.Init(direction);
        }

        private Vector2? FindNearestEnemyPosition(Vector2 origin)
        {
            int count = Physics2D.OverlapCircle(origin, aimRadius, _contactFilter, HitBuffer);
            if (count == 0) return null;

            float minDist = float.MaxValue;
            Vector2 closest = origin;

            for (int i = 0; i < count; i++)
            {
                float dist = Vector2.Distance(origin, HitBuffer[i].transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = HitBuffer[i].transform.position;
                }
            }

            return closest;
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            return _camera.ScreenToWorldPoint(mouseScreen);
        }
    }
}
