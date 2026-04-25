using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private const float MoveScaleAmount = 1.1f;
        private const float MoveScaleDuration = 0.5f;
        private const float HitColorDuration = 0.5f;
        private const float HitShakeDuration = 0.1f;
        private const float HitShakeStrength = 0.1f;

        private SpriteRenderer _sr;
        private Tweener _tween;
        private Tweener _bounceTween;
        private Tweener _hitTween;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private Tween _walkTween;

        public void SetMoving(bool moving)
        {
            if (moving)
            {
                // Если прыжок уже идет — не мешаем
                if (_walkTween != null && _walkTween.IsActive()) return;

                // Делаем прыжок на месте относительно текущей позиции
                // 0.15f — высота прыжка, 1 — количество прыжков, 0.3f — длительность
                _walkTween = transform.DOJump(G.Player.transform.position, 0.15f, 1, 0.3f)
                    .SetRelative(true) 
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetLink(gameObject);
            }
            else
            {
                if (_walkTween != null)
                {
                    _walkTween.Kill();
                    _walkTween = null;
                    // Возвращаем в исходный скейл/позицию, если нужно
                    transform.DOLocalMoveY(0, 0.1f); 
                }
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