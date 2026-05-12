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

    private static Sprite _pixelSprite;

    private static Sprite PixelSprite
    {
        get
        {
            if (_pixelSprite != null) return _pixelSprite;
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
            };
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            _pixelSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            return _pixelSprite;
        }
    }

    private void PlayExplosion()
    {
        Vector3 deathPos = transform.position;
        int sortingLayer = _sr != null ? _sr.sortingLayerID : 0;
        int sortingOrder = _sr != null ? _sr.sortingOrder : 0;

        SpawnSpot(deathPos, sortingLayer);
        SpawnPixels(deathPos, sortingLayer, sortingOrder);
    }

    private static void SpawnPixels(Vector3 pos, int layer, int order)
    {
        const int count = 14;
        for (int i = 0; i < count; i++)
        {
            var shard = new GameObject("EnemyPixel");
            shard.transform.position = pos;
            float pixelScale = Random.Range(0.05f, 0.09f);
            shard.transform.localScale = new Vector3(pixelScale, pixelScale, 1f);

            var ssr = shard.AddComponent<SpriteRenderer>();
            ssr.sprite = PixelSprite;
            float r = Random.Range(0.35f, 0.55f);
            float g = Random.Range(0.0f, 0.05f);
            float b = Random.Range(0.0f, 0.05f);
            ssr.color = new Color(r, g, b, 1f);
            ssr.sortingLayerID = layer;
            ssr.sortingOrder = order + 1;

            float angle = i * Mathf.PI * 2f / count + Random.Range(-0.4f, 0.4f);
            Vector2 dir = new(Mathf.Cos(angle), Mathf.Sin(angle));
            float dist = Random.Range(0.6f, 1.4f);
            float duration = Random.Range(0.35f, 0.6f);

            var capturedShard = shard;
            shard.transform.DOMove(pos + (Vector3)(dir * dist), duration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => Destroy(capturedShard));
        }
    }

    private static void SpawnSpot(Vector3 pos, int layer)
    {
        const int clusterCount = 20;
        const float clusterRadius = 0.25f;
        for (int i = 0; i < clusterCount; i++)
        {
            float r = Mathf.Sqrt(Random.value) * clusterRadius;
            float a = Random.value * Mathf.PI * 2f;
            Vector3 offset = new(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0f);

            var dot = new GameObject("BloodDot");
            dot.transform.position = pos + offset;
            float scale = Random.Range(0.05f, 0.09f);
            dot.transform.localScale = new Vector3(scale, scale, 1f);

            var sr = dot.AddComponent<SpriteRenderer>();
            sr.sprite = PixelSprite;
            sr.color = new Color(Random.Range(0.25f, 0.45f), Random.Range(0.0f, 0.04f), Random.Range(0.0f, 0.04f), 1f);
            sr.sortingLayerID = layer;
            sr.sortingOrder = 0;
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