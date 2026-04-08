using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Events
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Foundation/Events/GameEvent")]
    public class GameEvent<T> : ScriptableObject
    {
        private readonly List<GameEventListener<T>> _listeners = new();

        public void Raise(T value)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
            }
        }

        public void Register(GameEventListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void Unregister(GameEventListener<T> listener)
        {
            _listeners.Remove(listener);
        }
    }
}
