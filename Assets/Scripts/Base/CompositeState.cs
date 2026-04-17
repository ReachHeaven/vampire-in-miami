using System;
using System.Collections.Generic;

namespace Base
{
    [Serializable]
    public class CompositeStateEntity
    {
    }

    public class CompositeState
    {
        public List<CompositeStateEntity> components = new(4);

        public T Get<T>() where T : CompositeStateEntity, new()
        {
            foreach (var c in components)
            {
                if (c is T ct)
                    return ct;
            }

            var newState = new T();
            components.Add(newState);
            return newState;
        }
    }

    public class ObjectState : CompositeState
    {
        public CMSEntity Model;
        public ViewBase View;

        public T Tag<T>() where T : EntityComponentDefinition, new()
            => Model.Get<T>();

        public bool Is<T>(out T t) where T : EntityComponentDefinition, new()
            => Model.Is<T>(out t);

        public void DestroySafely()
        {
            if (View != null)
                UnityEngine.Object.Destroy(View.gameObject);
        }
    }
}
