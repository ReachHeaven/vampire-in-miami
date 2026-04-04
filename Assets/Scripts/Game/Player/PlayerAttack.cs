using Game.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float cooldown = 1;
        [SerializeField] private Bullet bulletPrefab;
        private float _lastShotTime;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
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
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseWorld = _camera.ScreenToWorldPoint(currentMousePosition);
            Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;
            
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Init(direction);
        }
    }
}