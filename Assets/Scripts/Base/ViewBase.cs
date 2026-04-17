using UnityEngine;

namespace Base
{
    public class ViewBase : MonoBehaviour
    {
        protected bool interactable = true;

        public void SetInteractable(bool state) =>  interactable = state;

        public T As<T>() where T : ViewBase
        {
            return this as T;
        }
    }
}