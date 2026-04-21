using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Tweener _tween;
        private Tweener _hitTween;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void SetMoving(bool moving)
        {
            if (moving && _tween != null)
            {
                _tween = transform.DOScale(Vector3.one * 0.1f, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .SetLink(gameObject);
            }
            else if (!moving && _tween != null)
            {
                _tween.Kill();
                _tween = null;
                transform.localScale = Vector3.one;
            }
        }

        public void PlayHit()
        {
            _hitTween?.Kill();
            _sr.color = Color.white;
            _hitTween = _sr.DOColor(Color.red, 0.5f)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject);

            transform.DOShakePosition(0.1f, 0.1f);
        }
    }
}