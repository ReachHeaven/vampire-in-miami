using Base;
using UnityEngine;

public class EnemyState : ObjectState
{
    public int Health;
    public float Speed;
    public int ContactDamage;
    public bool IsDead => Health <= 0;

    public EnemyState(CMSEntity model)
    {
        Model = model;
        var stats = model.Get<TagStats>();
        Health = stats.MaxHealth;
        Speed = stats.Speed;
        ContactDamage = model.Is<TagContactDamage>(out var d) ? d.Damage : 0;
    }

    public void ApplyDamage(int dmg) =>
        Health = Mathf.Max(0, Health - dmg);
}
