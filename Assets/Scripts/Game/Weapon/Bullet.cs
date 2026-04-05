using Game.Core;
using UnityEngine;

namespace Game.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int bulletDamage = 30;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (bulletSpeed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Health health))
            {
                health.TakeDamage(bulletDamage);
            }
        }

        public void Init(Vector2 direction)
        {
            _direction = direction;
            Destroy(gameObject, 5f);
        }
    }
}