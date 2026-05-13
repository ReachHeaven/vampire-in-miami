using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class IntroView : ModalViewBase
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private string[] _lines;
        [SerializeField] private float _typeStep = 0.135f;
        [SerializeField] private int _typeTicksPerLine = 4;
        [SerializeField] private float _holdAfterType = 1.5f;
        [SerializeField, FormerlySerializedAs("_lineFade")] private float _lineFadeDuration = 0.7f;

        public async UniTask Play(bool chainNext = false, Action onBeforeFade = null)
        {
            if (_skip || _root == null || _label == null || _lines == null || _lines.Length == 0)
            {
                if (_root != null) _root.gameObject.SetActive(false);
                onBeforeFade?.Invoke();
                return;
            }

            ShowRoot();

            for (int i = 0; i < _lines.Length; i++)
            {
                bool isLast = i == _lines.Length - 1;
                await WriteLine(_lines[i], withFade: !isLast);
            }

            onBeforeFade?.Invoke();

            await FadeRoot(0f);
            FinishHide(chainNext);
        }

        private async UniTask WriteLine(string text, bool withFade)
        {
            _label.color = Color.white;
            _label.text = text;

            for (int i = 0; i < _typeTicksPerLine; i++)
                await UniTask.WaitForSeconds(_typeStep, ignoreTimeScale: true);

            await UniTask.WaitForSeconds(_holdAfterType, ignoreTimeScale: true);

            if (withFade)
                await _label.DOFade(0f, _lineFadeDuration).SetEase(Ease.OutSine)
                    .SetUpdate(true).AsyncWaitForCompletion();
        }
    }
}
