using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Patterns
{
    /// <summary>
    /// Simple state machine. Register states by type, switch between them.
    /// </summary>
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new();
        private IState _currentState;

        /// <summary>
        /// Register a state instance by its concrete type.
        /// </summary>
        public void AddState<T>(T state) where T : class, IState
        {
            _states[typeof(T)] = state;
        }

        /// <summary>
        /// Switch to a registered state. Calls Exit on current, Enter on new.
        /// </summary>
        public void SwitchTo<T>() where T : class, IState
        {
            Type type = typeof(T);

            if (!_states.TryGetValue(type, out IState next))
            {
                Debug.LogError($"[StateMachine] State {type.Name} not registered.");
                return;
            }

            _currentState?.Exit();
            _currentState = next;
            _currentState.Enter();
        }

        /// <summary>
        /// Call every frame from MonoBehaviour.Update().
        /// </summary>
        public void Update()
        {
            _currentState?.Update();
        }
    }
}
