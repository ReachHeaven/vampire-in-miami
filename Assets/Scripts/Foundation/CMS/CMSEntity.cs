using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.CMS
{
    [Serializable]
    public class CMSEntity
    {
        public string id;
        [SerializeReference, SubclassSelector] public List<EntityComponentDefinition> components = new();


        public T Define<T>() where T : EntityComponentDefinition, new()
        {
            var component = Get<T>();
            
            if (component != null)
            {
                return component;
            }
            
            component = new T();
            components.Add(component);
            return component;
        }
        
        public T Get<T>() where T : EntityComponentDefinition, new()
        {
            return components.Find(x => x is T) as T;
        }
        
        public bool Is<T>() where T : EntityComponentDefinition, new()
        {
            return Get<T>() != null;
        }

        public bool Is<T>(out T component) where T : EntityComponentDefinition, new()
        {
            component = Get<T>();
            return component != null;
        }
    }
}