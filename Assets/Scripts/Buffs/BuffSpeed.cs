using System;

namespace Buffs
{
    [Serializable]
    public class BuffSpeed : IAction
    {
        
        public string Name => "Speed buff";

        public void Execute()
        {
            G.Player.State.Speed += 10;
        }
    }
}