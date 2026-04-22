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
        public int MaxLevel = 10;

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

        public bool TryGetLevel(int experience)
        {
            if(Level >= MaxLevel)
                return false;
            
            Experience += experience;
            bool leveled = false;

            while (Experience >= ExperienceToNextLevel)
            {
                Level += 1;
                Experience -= ExperienceToNextLevel;
                leveled = true;
            }

            return leveled;
        }

        public void ApplyDamage(int dmg) =>
            Health = Mathf.Max(0, Health - dmg);

        public void Heal(int amount) =>
            Health = Mathf.Min(Health + amount, MaxHealth);
    }
}