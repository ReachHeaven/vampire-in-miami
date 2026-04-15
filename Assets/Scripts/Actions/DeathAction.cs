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
            UnityEngine.Object.Destroy(actor);
        }
    }
    
    [Serializable]
    public class EnemyDeathAction : TagDeathAction
    {
        public override void OnKill(GameObject actor)
        {
            G.WaveSystem.NotifyEnemyKilled();
            UnityEngine.Object.Destroy(actor);
        }
    }
}