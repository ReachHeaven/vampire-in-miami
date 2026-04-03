using UnityEngine;
using UnityEngine.Events;

namespace Foundation.Events
{
    /// <summary>
    /// MonoBehaviour that listens to a GameEvent and invokes a UnityEvent response.
    /// Attach to any GameObject, assign event and response in Inspector.
    /// </summary>
    public class GameEventListener<T> : MonoBehaviour
    {
        [SerializeField]
        private GameEvent<T> gameEvent;

        [SerializeField]
        private UnityEvent<T> response;

        private void OnEnable()
        {
            gameEvent?.Register(this);
        }

        private void OnDisable()
        {
            gameEvent?.Unregister(this);
        }

        public void OnEventRaised(T value)
        {
            response?.Invoke(value);
        }
    }
}
