using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Events
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Assets/Foundation/Events/GameEvent.cs")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public void Register(GameEventListener listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void Unregister(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}
