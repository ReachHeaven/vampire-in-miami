using System.Collections.Generic;
using UnityEngine;

namespace Foundation.CMS
{
    public class CMSEntityPfb : MonoBehaviour
    {
        public string idCMS;

        [SerializeReference, SubclassSelector] 
        public List<EntityComponentDefinition> Components = new();

        public string GetId() => idCMS;

        public CMSEntity AsEntity() => CMS.Get<CMSEntity>(GetId());

        public T As<T>() where T : EntityComponentDefinition, new()
            => AsEntity().Get<T>();
    }
}