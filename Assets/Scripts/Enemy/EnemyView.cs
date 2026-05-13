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
    private float _moveTime;

    private void Awake()
    {
        transform.DOScale(Vector3.one * 1.02f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _moveTime = Random.value * 10f;
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
        _moveTime += Time.fixedDeltaTime;

        Vector2 toTarget = (Vector2)_target.position - _rb.position;
        Vector2 forward = toTarget.sqrMagnitude > 1e-6f ? toTarget.normalized : Vector2.zero;
        Vector2 dir = forward;
        float speedMul = 1f;

        var m = State.Movement;
        if (m != null)
        {
            switch (m.Kind)
            {
                case MovementKind.Zigzag:
                {
                    Vector2 perp = new(-forward.y, forward.x);
                    float lateral = Mathf.Sin(_moveTime * m.ZigzagFrequency) * m.ZigzagAmplitude;
                    Vector2 mixed = forward + perp * lateral;
                    if (mixed.sqrMagnitude > 1e-6f) dir = mixed.normalized;
                    break;
                }
                case MovementKind.Dash:
                {
                    float cycle = m.DashCooldownSeconds + m.DashWindupSeconds + m.DashChargeSeconds;
                    if (cycle > 0f)
                    {
                        float t = _moveTime % cycle;
                        if (t < m.DashCooldownSeconds) speedMul = m.DashIdleSpeedMul;
                        else if (t < m.DashCooldownSeconds + m.DashWindupSeconds) speedMul = 0f;
                        else speedMul = m.DashChargeSpeedMul;
                    }
                    break;
                }
            }
        }

        _rb.MovePosition(_rb.position + dir * (State.Speed * speedMul * Time.fixedDeltaTime));
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