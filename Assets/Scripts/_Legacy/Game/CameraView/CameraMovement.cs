using UnityEngine;

namespace Game.CameraView
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5f;

        private void LateUpdate()
        {
            if (!target) return;

            var position = new Vector3(target.position.x, target.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
        }
    }
}