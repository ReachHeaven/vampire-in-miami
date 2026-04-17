using Base;
using UnityEngine;

namespace Player
{
    public class PlayerState : ObjectState
    {
        public int Health;
        public float Speed;
        public int Level = 0;
        public TagWeapon Weapon;
        public int Experience;
        public int ExperienceToNextLevel = 100;

        public bool IsDead => Health <= 0;

        public PlayerState(CMSEntity model)
        {
            Model = model;
            var stats = model.Get<TagStats>();
            Health = stats.MaxHealth;
            Speed = stats.Speed;
            Weapon = model.Get<TagWeapon>();
        }

        public void TryGetLevel(int experience)
        {
            Experience += experience;
            if (Experience >= ExperienceToNextLevel)
            {
                Level += 1;
                Experience = 0;
            }
        }

        public void ApplyDamage(int dmg) =>
            Health = Mathf.Max(0, Health - dmg);

        public void Heal(int amount, int max) =>
            Health = Mathf.Min(Health + amount, max);
    }
}
