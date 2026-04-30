using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class IntroView : MonoBehaviour
    {
        [SerializeField] private bool _skip;
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private string[] _lines;
        [SerializeField] private float _typeStep = 0.135f;
        [SerializeField] private int _typeTicksPerLine = 4;
        [SerializeField] private float _holdAfterType = 1.5f;
        [SerializeField] private float _lineFade = 0.7f;
        [SerializeField] private float _rootFade = 0.4f;
        [SerializeField] private GameObject[] _hideWhilePlaying;

        private void Awake()
        {
            if (_root != null) _root.gameObject.SetActive(false);
        }

        public async UniTask Play()
        {
            if (_skip || _root == null || _label == null || _lines == null || _lines.Length == 0)
            {
                if (_root != null) _root.gameObject.SetActive(false);
                return;
            }

            SetHidden(true);
            Time.timeScale = 0f;

            _root.gameObject.SetActive(true);
            _root.alpha = 1f;

            for (int i = 0; i < _lines.Length; i++)
            {
                bool isLast = i == _lines.Length - 1;
                await WriteLine(_lines[i], withFade: !isLast);
            }

            await _root.DOFade(0f, _rootFade).SetEase(Ease.OutSine)
                .SetUpdate(true).AsyncWaitForCompletion();
            _root.gameObject.SetActive(false);

            Time.timeScale = 1f;
            SetHidden(false);
        }

        private void SetHidden(bool hidden)
        {
            if (_hideWhilePlaying == null) return;
            foreach (var go in _hideWhilePlaying)
                if (go != null) go.SetActive(!hidden);
        }

        private async UniTask WriteLine(string text, bool withFade)
        {
            _label.color = Color.white;
            _label.text = text;

            for (int i = 0; i < _typeTicksPerLine; i++)
                await UniTask.WaitForSeconds(_typeStep, ignoreTimeScale: true);

            await UniTask.WaitForSeconds(_holdAfterType, ignoreTimeScale: true);

            if (withFade)
                await _label.DOFade(0f, _lineFade).SetEase(Ease.OutSine)
                    .SetUpdate(true).AsyncWaitForCompletion();
        }
    }
}
