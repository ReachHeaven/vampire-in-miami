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
        PlayExplosion();
        Destroy(gameObject);
    }

    private void PlayExplosion()
    {
        Vector3 deathPos = transform.position;
        Sprite sprite = _sr != null ? _sr.sprite : null;
        Vector3 baseScale = transform.localScale;
        int sortingLayer = _sr != null ? _sr.sortingLayerID : 0;
        int sortingOrder = _sr != null ? _sr.sortingOrder : 0;

        SpawnFlash(deathPos, sprite, baseScale, sortingLayer, sortingOrder);
        SpawnShards(deathPos, sprite, baseScale, sortingLayer, sortingOrder);
    }

    private static void SpawnFlash(Vector3 pos, Sprite sprite, Vector3 baseScale, int layer, int order)
    {
        if (sprite == null) return;
        var ghost = new GameObject("EnemyExplosionFlash");
        ghost.transform.position = pos;
        ghost.transform.localScale = baseScale;
        var sr = ghost.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.white;
        sr.sortingLayerID = layer;
        sr.sortingOrder = order + 1;

        ghost.transform.DOScale(baseScale * 2.5f, 0.35f).SetEase(Ease.OutQuad);
        sr.DOFade(0f, 0.35f).SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(ghost));
    }

    private static void SpawnShards(Vector3 pos, Sprite sprite, Vector3 baseScale, int layer, int order)
    {
        if (sprite == null) return;
        const int count = 6;
        for (int i = 0; i < count; i++)
        {
            var shard = new GameObject("EnemyShard");
            shard.transform.position = pos;
            shard.transform.localScale = baseScale * 0.35f;
            var ssr = shard.AddComponent<SpriteRenderer>();
            ssr.sprite = sprite;
            ssr.color = new Color(1f, 0.85f, 0.6f, 1f);
            ssr.sortingLayerID = layer;
            ssr.sortingOrder = order;

            float angle = i * Mathf.PI * 2f / count + Random.Range(-0.3f, 0.3f);
            Vector2 dir = new(Mathf.Cos(angle), Mathf.Sin(angle));
            float dist = Random.Range(0.7f, 1.3f);
            float duration = Random.Range(0.35f, 0.55f);

            shard.transform.DOMove(pos + (Vector3)(dir * dist), duration).SetEase(Ease.OutCubic);
            shard.transform.DORotate(new Vector3(0, 0, Random.Range(-540f, 540f)), duration);
            ssr.DOFade(0f, duration).SetEase(Ease.InQuad)
                .OnComplete(() => Destroy(shard));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerView>(out var player))
        {
            player.TakeDamage(State.ContactDamage);
            G.Player.PlayerAnimation.PlayHit();
        }
    }
}