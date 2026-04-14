using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Foundation.CMS
{
    [Serializable]
    public class CMSEntity
    {
        public string id;

        [SerializeReference]
        public List<EntityComponentDefinition> components = new();

        public T Define<T>() where T : EntityComponentDefinition, new()
        {
            var component = Get<T>();
            if (component != null) return component;

            component = new T();
            components.Add(component);
            return component;
        }

        public T Get<T>() where T : EntityComponentDefinition
        {
            return components.Find(x => x is T) as T;
        }

        public bool Is<T>() where T : EntityComponentDefinition
        {
            return Get<T>() != null;
        }

        public bool Is<T>(out T component) where T : EntityComponentDefinition
        {
            component = Get<T>();
            return component != null;
        }

        public CMSEntity DeepCopy()
        {
            var clone = new CMSEntity();
            clone.id = id;
            clone.components = new List<EntityComponentDefinition>(components.Count);

            foreach (var component in components)
                clone.components.Add(CloneComponent(component));

            return clone;
        }

        private static EntityComponentDefinition CloneComponent(EntityComponentDefinition source)
        {
            var type = source.GetType();
            var clone = (EntityComponentDefinition)Activator.CreateInstance(type);

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
                field.SetValue(clone, field.GetValue(source));

            return clone;
        }
    }
}