using Actions;
using UnityEngine;

namespace Systems
{
    public class HealthSystem : MonoBehaviour, IService
    {
        public void Init()
        {
            Debug.Log($"{this} Init");
        }

        public void ApplyDamage(CMSEntity entity, int damage, MonoBehaviour source)
        {
            if (!entity.Is<TagStats>(out var stats)) return;

            stats.Health -= damage;
            if (stats.Health <= 0) Kill(entity, source);
        }

        public void Heal(CMSEntity entity, int restored)
        {
            if (!entity.Is<TagStats>(out var stats)) return;
            if (stats.Health <= 0) return;

            stats.Health = Mathf.Min(stats.Health + restored, stats.MaxHealth);
        }

        public void Kill(CMSEntity entity, MonoBehaviour source)
        {
            Debug.Log($"{this} Killed");
            if (entity.IsAbstract<TagDeathAction>(out var actor))
            {
                actor.OnKill(source.gameObject);
            }
        }
    }
}