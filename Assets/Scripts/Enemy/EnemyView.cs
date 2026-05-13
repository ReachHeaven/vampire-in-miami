using Base;
using Base.Player;
using DG.Tweening;
using Runtime;
using UnityEngine;

public class EnemyView : ViewBase
{
    public EnemyState State;
    private Transform _target;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Awake()
    {
        transform.DOScale(Vector3.one * 1.02f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Init(EnemyState state)
    {
        State = state;
        State.View = this;

        if (State.Model.Is<TagSprite>(out var tagSprite))
            _sr.sprite = tagSprite.sprite;
    }

    public void SetTarget(Transform target)
    {
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
        if (State.IsDead) return;
        State.ApplyDamage(damage);
        if (!State.IsDead) return;

        G.Waves.NotifyKilled(this);
        if (G.Player != null)
        {
            bool leveled = G.Player.State.TryGetLevel(State.ExperienceGained);
            G.Hud.SetExperience(G.Player.State.Experience, G.Player.State.ExperienceToNextLevel);
            G.Hud.SetLevel(G.Player.State.Level);
            if (leveled) G.GameMain.OnLevelUp();
        }
        BloodEffect.Play(transform.position, _sr);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<PlayerView>(out var player)) return;
        player.TakeDamage(State.ContactDamage);
        if (G.Player != null) G.Player.PlayerAnimation.PlayHit();
    }
}