using System;
using Base;
using UnityEngine;

namespace Player
{
    public class PlayerState : ObjectState
    {
        public int Health;
        public int MaxHealth;
        public float Speed;
        public int Level;
        public int Experience;
        public int ExperienceToNextLevel;

        public WeaponState Weapon;

        public bool HasWeapon => Weapon != null;
        public bool IsDead => Health <= 0;

        public PlayerState(CMSEntity model)
        {
            Model = model;
            var stats = model.Get<TagStats>();
            MaxHealth = stats.MaxHealth;
            Health = MaxHealth;
            Speed = stats.Speed;
            ExperienceToNextLevel = model.Get<TagExperience>().ExperienceToNextLevel;

            if (model.Is<TagEquippedWeapon>(out var equipped) && equipped.WeaponPfb)
                Weapon = new WeaponState(equipped.WeaponPfb.AsEntity());
        }

        public void TryGetLevel(int experience)
        {
            Experience += experience;

            while (Experience >= ExperienceToNextLevel)
            {
                Level += 1;
                Experience -= ExperienceToNextLevel;
            }
        }

        public void ApplyDamage(int dmg) =>
            Health = Mathf.Max(0, Health - dmg);

        public void Heal(int amount) =>
            Health = Mathf.Min(Health + amount, MaxHealth);
    }
}