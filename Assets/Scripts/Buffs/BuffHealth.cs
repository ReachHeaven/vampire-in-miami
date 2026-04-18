using System;

namespace Buffs
{
    [Serializable]
    public class BuffHealth : IAction
    {
        
        public string Name => "Health buff";

        public void Execute()
        {
            G.Player.State.Health += 10;
        }
    }
}