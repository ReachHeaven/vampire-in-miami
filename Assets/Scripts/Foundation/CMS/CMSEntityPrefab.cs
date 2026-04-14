using System.Collections.Generic;
using UnityEngine;

namespace Foundation.CMS
{
    public class CMSEntityPrefab : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        public List<EntityComponentDefinition> Components = new();

        public T Get<T>() where T : EntityComponentDefinition
        {
            foreach (var c in Components)
                if (c is T tag) return tag;
            return null;
        }

        public bool Has<T>() where T : EntityComponentDefinition
            => Get<T>() != null;
    }
}