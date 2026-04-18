using System;

namespace Buffs
{
    [Serializable]
    public class BuffDamage : IAction
    {
        public string Name => "Damage buff";

        public void Execute()
        {
            if (G.Player.State.HasWeapon)
                G.Player.State.Weapon.Damage += 10;
        }
    }
}