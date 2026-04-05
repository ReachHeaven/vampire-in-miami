using UnityEngine;

namespace Foundation.Events
{
    [CreateAssetMenu(fileName = "NewFloatEvent", menuName = "Foundation/Events/Float Event")]
    public class FloatGameEvent : GameEvent<float> { }
    public class FloatGameEventListener : GameEventListener<float> { }
}
