using System;

namespace Buffs
{
    [Serializable]
    public class BuffHealth : IAction
    {
        
        public string Name => "Health buff";

        public void Execute()
        {
            var state = G.Player.State;
            state.MaxHealth += 10;
            state.Heal(10);
        }
    }
}