using System;
using UnityEngine;

namespace Utils
{
    public class MoveableBase : MonoBehaviour
    {
        public Vector3 TargetPosition;
        public float SmoothTime;

        private Vector3 Velocity;

        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, SmoothTime);
        }
    }
}