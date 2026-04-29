using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private const float HitColorDuration = 0.5f;
        private const float HitShakeDuration = 0.1f;
        private const float HitShakeStrength = 0.1f;

        private SpriteRenderer _sr;
        private Tweener _hitTween;
        private Tween _walkTween;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>(true);
        }

        public void SetMoving(bool moving)
        {
            if (moving)
            {
                if (_walkTween != null && _walkTween.IsActive()) return;

                _walkTween = transform.DOJump(transform.position, 0.15f, 1, 0.3f)
                    .SetRelative(true)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetLink(gameObject);
            }
            else if (_walkTween != null)
            {
                _walkTween.Kill();
                _walkTween = null;
                transform.DOLocalMoveY(0, 0.1f);
            }
        }

        public void PlayHit()
        {
            _hitTween?.Kill();
            _sr.color = Color.white;
            _hitTween = _sr.DOColor(Color.red, HitColorDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject);

            transform.DOShakePosition(HitShakeDuration, HitShakeStrength);
        }
    }
}