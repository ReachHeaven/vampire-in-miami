using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buffs
{
    [Serializable]
    public class TagBuffs : EntityComponentDefinition
    {
        [SerializeReference, SubclassSelector] 
        public List<IAction> Buffs;
    }

    public interface IAction
    {
        string Name { get; }
        void Execute();
    }
}