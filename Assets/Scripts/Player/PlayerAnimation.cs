using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Vector3 _baseScale;
        private float _bobTimer;
        private bool _isMoving;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _baseScale = transform.localScale;
            _sr = GetComponent<SpriteRenderer>();
        }

        public void SetMoving(bool moving)
        {
            _isMoving = moving;
            if (!moving)
            {
                transform.localScale = _baseScale;
                _bobTimer = 0;
            }
        }

        public void PlayHit()
        {
            _sr.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        private void ResetColor()
        {
            _sr.color = Color.white;
        }

        private void Update()
        {
            if (!_isMoving) return;

            _bobTimer += Time.deltaTime * 10f;
            float squash = 1f + Mathf.Sin(_bobTimer) * 0.1f;
            transform.localScale = new Vector3(
                _baseScale.x / squash,
                _baseScale.y * squash,
                _baseScale.z
            );
        }
    }
}