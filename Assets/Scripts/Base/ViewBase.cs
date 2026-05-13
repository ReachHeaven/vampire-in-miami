using UnityEngine;

namespace Base
{
    public class ViewBase : MonoBehaviour
    {
        public T As<T>() where T : ViewBase
        {
            return this as T;
        }
    }
}