using Base;
using UnityEngine;

namespace Player
{
    public class PlayerState : ObjectState
    {
        public int Health;
        public float Speed;
        public TagWeapon Weapon;
        public bool IsDead => Health <= 0;

        public PlayerState(CMSEntity model)
        {
            Model = model;
            var stats = model.Get<TagStats>();
            Health = stats.MaxHealth;
            Speed = stats.Speed;
            Weapon = model.Get<TagWeapon>();
        }

        public void ApplyDamage(int dmg) =>
            Health = Mathf.Max(0, Health - dmg);

        public void Heal(int amount, int max) =>
            Health = Mathf.Min(Health + amount, max);
    }
}
