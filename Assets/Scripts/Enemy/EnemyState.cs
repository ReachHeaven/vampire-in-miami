using Base;
using Runtime;
using UnityEngine;

public class EnemyState : ObjectState
{
    public int Health;
    public float Speed;
    public int ContactDamage;
    public int ExperienceGained;
    public Sprite sprite;
    public bool IsDead => Health <= 0;

    public EnemyState(CMSEntity model)
    {
        Model = model;
        var stats = model.Get<TagStats>();
        Health = stats.MaxHealth;
        Speed = stats.Speed;
        sprite = model.Is<TagSprite>() ? model.Get<TagSprite>().sprite : null;
        ContactDamage = model.Is<TagContactDamage>(out var d) ? d.Damage : 0;
        ExperienceGained = model.Is<TagExperienceDrop>(out var drop) ? drop.Amount : 0;
    }

    public void ApplyDamage(int dmg) =>
        Health = Mathf.Max(0, Health - dmg);
}
