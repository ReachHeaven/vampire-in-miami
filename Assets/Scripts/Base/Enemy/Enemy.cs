using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CMSEntity _instance;
    public TagStats _stats;
    public Transform _target;
    public Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(CMSEntity definition, Transform target)
    {
        _instance = definition.DeepCopy();
        _stats = _instance.Get<TagStats>();
        _target = target;
    }

    private void FixedUpdate()
    {
        if (!_target) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.MovePosition(_rb.position + direction * (_stats.Speed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage)
    {
        G.HealthSystem.ApplyDamage(_instance, damage, this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Base.Player.Player>(out var player))
        {
            if (_instance.Is<TagContactDamage>(out var dmg))
                player.TakeDamage(dmg.Damage);
        }
    }
}