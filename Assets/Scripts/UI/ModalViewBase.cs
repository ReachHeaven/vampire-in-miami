using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI
{
    public abstract class ModalViewBase : MonoBehaviour
    {
        [SerializeField] protected bool _skip;
        [SerializeField] protected CanvasGroup _root;
        [SerializeField, FormerlySerializedAs("_rootFade"), FormerlySerializedAs("_fadeIn")]
        protected float _fadeDuration = 0.4f;
        [SerializeField] protected GameObject[] _hideWhilePlaying;

        protected virtual void Awake()
        {
            if (_root != null) _root.gameObject.SetActive(false);
        }

        protected void ShowRoot(float alpha = 1f)
        {
            SetHidden(true);
            Time.timeScale = 0f;
            _root.gameObject.SetActive(true);
            _root.alpha = alpha;
        }

        protected Task FadeRoot(float to) =>
            _root.DOFade(to, _fadeDuration).SetEase(Ease.OutSine)
                .SetUpdate(true).AsyncWaitForCompletion();

        protected void FinishHide(bool chainNext)
        {
            _root.gameObject.SetActive(false);
            if (!chainNext)
            {
                Time.timeScale = 1f;
                SetHidden(false);
            }
        }

        protected void SetHidden(bool hidden)
        {
            if (_hideWhilePlaying == null) return;
            foreach (var go in _hideWhilePlaying)
                if (go != null) go.SetActive(!hidden);
        }

        protected static bool AnyKeyPressed()
        {
            var kb = Keyboard.current;
            if (kb != null && kb.anyKey.wasPressedThisFrame) return true;
            var m = Mouse.current;
            return m != null && (m.leftButton.wasPressedThisFrame || m.rightButton.wasPressedThisFrame);
        }
    }
}
