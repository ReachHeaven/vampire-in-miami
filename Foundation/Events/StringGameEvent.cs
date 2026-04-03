using UnityEngine;

namespace Foundation.Events
{
    [CreateAssetMenu(fileName = "NewStringEvent", menuName = "Foundation/Events/String Event")]
    public class StringGameEvent : GameEvent<string> { }
    public class StringGameEventListener : GameEventListener<string> { }
}
