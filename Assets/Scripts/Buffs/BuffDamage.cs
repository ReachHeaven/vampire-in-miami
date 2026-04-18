using System;

namespace Buffs
{
    [Serializable]
    public class BuffDamage : IAction
    {
        public string Name => "Damage buff";

        public void Execute()
        {
            G.Player.State.Weapon.Damage += 10;
        }
    }
}