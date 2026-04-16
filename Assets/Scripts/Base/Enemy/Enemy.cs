using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CMSEntity Instance;
    public TagStats Stats;
    public Transform _target;
    public Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(CMSEntity definition, Transform target)
    {
        Instance = definition.DeepCopy();
        Stats = Instance.Get<TagStats>().DeepCopy();
        Stats.Health = Stats.MaxHealth;
        _target = target;
    }

    private void FixedUpdate()
    {
        if (!_target) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.MovePosition(_rb.position + direction * (Stats.Speed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage)
    {
        G.HealthSystem.ApplyDamage(Instance, damage, this);
        Debug.Log($"Damage is taken {Stats.Health}\\{Stats.MaxHealth}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Base.Player.Player>(out var player))
        {
            if (Instance.Is<TagContactDamage>(out var dmg))
                player.TakeDamage(dmg.Damage);
        }
    }
}