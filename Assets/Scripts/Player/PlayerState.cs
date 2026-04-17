using System;
using Base;
using UnityEngine;

namespace Player
{
    public class PlayerState : ObjectState
    {
        public int Health;
        public float Speed;
        public int Level;
        public int Experience;
        public int ExperienceToNextLevel;

        public bool IsDead => Health <= 0;

        public PlayerState(CMSEntity model)
        {
            Model = model;
            var stats = model.Get<TagStats>();
            Health = stats.MaxHealth;
            Speed = stats.Speed;
            ExperienceToNextLevel = model.Get<TagExperience>().ExperienceToNextLevel;
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

        public void Heal(int amount, int max) =>
            Health = Mathf.Min(Health + amount, max);
    }
}