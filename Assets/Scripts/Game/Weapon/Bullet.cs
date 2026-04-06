using Game.Core;
using UnityEngine;

namespace Game.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int bulletDamage = 30;

        private BulletModel _model;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _model = new BulletModel(bulletSpeed, bulletDamage);
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (_model.Speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Health health))
            {
                health.TakeDamage(_model.Damage);
            }
            Destroy(gameObject);
        }

        public void Init(Vector2 direction)
        {
            _direction = direction;
            Destroy(gameObject, 5f);
        }
    }
}
