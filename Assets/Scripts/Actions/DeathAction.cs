using System;
using UnityEngine;

namespace Actions
{
    [Serializable]
    public abstract class TagDeathAction : EntityComponentDefinition
    {
        public abstract void OnKill(GameObject actor);
    }

    [Serializable]
    public class PlayerDeathAction : TagDeathAction
    {
        public override void OnKill(GameObject actor)
        {
            Debug.Log("[Death] Player died");
            UnityEngine.Object.Destroy(actor);
        }
    }
    
    [Serializable]
    public class EnemyDeathAction : TagDeathAction
    {
        public override void OnKill(GameObject actor)
        {
            Debug.Log("[Death] Enemy died");
            var enemy = actor.GetComponent<Enemy>();
            G.WaveSystem.NotifyEnemyKilled(enemy);
            UnityEngine.Object.Destroy(actor);
        }
    }
}