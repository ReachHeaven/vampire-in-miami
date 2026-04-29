using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private const float HitColorDuration = 0.5f;

        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Transform _visual;
        private Tweener _hitTween;
        private Tween _walkTween;

        private Transform Visual => _visual != null ? _visual : transform;

        private void Awake()
        {
            if (_sr == null) _sr = GetComponentInChildren<SpriteRenderer>(true);
        }

        public void SetMoving(bool moving)
        {
            if (moving)
            {
                if (_walkTween != null && _walkTween.IsActive()) return;

                _walkTween = Visual.DOJump(Visual.position, 0.15f, 1, 0.3f)
                    .SetRelative(true)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetLink(gameObject);
            }
            else if (_walkTween != null)
            {
                _walkTween.Kill();
                _walkTween = null;
                Visual.DOLocalMoveY(0, 0.1f);
            }
        }

        public void PlayHit()
        {
            _hitTween?.Kill();
            if (_sr == null) return;
            _sr.color = Color.white;
            _hitTween = _sr.DOColor(Color.red, HitColorDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}