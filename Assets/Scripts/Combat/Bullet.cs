using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Vector2 _direction;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * (_speed * Time.fixedDeltaTime));
    }

    public void Init(Vector2 direction, int damage, float speed)
    {
        _direction = direction;
        _damage = damage;
        _speed = speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyView>(out var enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    public static void Spawn(TagWeapon weapon, Vector2 origin, Vector2 dir)
    {
        if (!weapon.bullet) return;

        var go = Instantiate(weapon.bullet.gameObject, origin, Quaternion.identity);
        var bullet = go.GetComponent<Bullet>();
        bullet.Init(dir, weapon.Damage, weapon.BulletSpeed);
    }
}
