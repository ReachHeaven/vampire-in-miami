using Base;
using Base.Player;
using UnityEngine;

public class EnemyView : ViewBase
{
    public EnemyState State;
    public int ExperienceThrown = 50;
    private Transform _target;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(CMSEntity model, Transform target)
    {
        State = new EnemyState(model);
        State.View = this;
        _target = target;
    }

    private void FixedUpdate()
    {
        if (!_target) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.MovePosition(_rb.position + direction * (State.Speed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Enemy get {damage} damage, health: {State.Health}. Zombie isDead : {State.IsDead}");
        State.ApplyDamage(damage);

        if (State.IsDead)
        {
            G.Waves.NotifyKilled(this);
            G.Player.State.TryGetLevel(State.ExpirienceGained);
            G.Hud.SetExperience(G.Player.State.Experience, G.Player.State.ExperienceToNextLevel);
            G.Hud.SetLevel(G.Player.State.Level);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerView>(out var player))
            player.TakeDamage(State.ContactDamage);
    }
}
