using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [Serializable]
    public class DialogLine
    {
        [TextArea] public string Text;
        public bool AskNameAfter;
    }

    public class DialogView : MonoBehaviour
    {
        private const string PlayerNameKey = "PlayerName";

        [SerializeField] private bool _skip;
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private DialogLine[] _lines;
        [SerializeField] private float _secondsPerChar = 0.04f;
        [SerializeField] private float _minTypeDuration = 0.3f;
        [SerializeField] private float _rootFade = 0.4f;
        [SerializeField] private GameObject[] _hideWhilePlaying;

        [Header("Head")]
        [SerializeField] private RectTransform _headRect;
        [SerializeField] private float _bobScale = 0.08f;
        [SerializeField] private float _bobInterval = 0.12f;

        [Header("Sound")]
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip[] _blips;
        [SerializeField] private int _charsPerBlip = 2;
        [SerializeField] private float _pitchMin = 0.85f;
        [SerializeField] private float _pitchMax = 1.15f;
        [SerializeField] private float _blipVolume = 0.6f;

        [Header("Name input")]
        [SerializeField] private GameObject _nameRoot;
        [SerializeField] private TMP_InputField _nameInput;

        private Tween _headTween;

        private void Awake()
        {
            if (_root != null) _root.gameObject.SetActive(false);
            if (_nameRoot != null) _nameRoot.SetActive(false);
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

            foreach (var line in _lines)
            {
                await Say(ResolvePlaceholders(line.Text));
                if (line.AskNameAfter) await AskName();
            }

            await _root.DOFade(0f, _rootFade).SetEase(Ease.OutSine)
                .SetUpdate(true).AsyncWaitForCompletion();
            _root.gameObject.SetActive(false);

            Time.timeScale = 1f;
            SetHidden(false);
        }

        private string ResolvePlaceholders(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string name = string.IsNullOrEmpty(G.PlayerName) ? "..." : G.PlayerName;
            return text.Replace("{name}", name);
        }

        private async UniTask Say(string text)
        {
            _label.text = string.Empty;
            float duration = Mathf.Max(_minTypeDuration, text.Length * _secondsPerChar);

            var cts = new CancellationTokenSource();
            BlipsLoop(text.Length, duration, cts.Token).Forget();
            StartHeadBob();

            float elapsed = 0f;
            int shown = 0;
            while (shown < text.Length)
            {
                if (Pressed())
                {
                    _label.text = text;
                    break;
                }
                elapsed += Time.unscaledDeltaTime;
                int target = Mathf.Clamp(Mathf.FloorToInt(elapsed / duration * text.Length), 0, text.Length);
                if (target != shown)
                {
                    shown = target;
                    _label.text = text.Substring(0, shown);
                }
                await UniTask.Yield();
            }

            cts.Cancel();
            StopHeadBob();
            await UniTask.NextFrame();

            await UniTask.WaitUntil(Pressed);
            await UniTask.NextFrame();
        }

        private async UniTaskVoid BlipsLoop(int charCount, float duration, CancellationToken ct)
        {
            if (_audio == null || _blips == null || _blips.Length == 0) return;
            int per = Mathf.Max(1, _charsPerBlip);
            int count = Mathf.Max(1, charCount / per);
            float interval = duration / count;

            for (int i = 0; i < count; i++)
            {
                if (ct.IsCancellationRequested) return;
                var clip = _blips[UnityEngine.Random.Range(0, _blips.Length)];
                _audio.pitch = UnityEngine.Random.Range(_pitchMin, _pitchMax);
                _audio.PlayOneShot(clip, _blipVolume);
                await UniTask.WaitForSeconds(interval, ignoreTimeScale: true,
                    cancellationToken: ct).SuppressCancellationThrow();
            }
        }

        private void StartHeadBob()
        {
            if (_headRect == null) return;
            _headTween?.Kill();
            _headRect.localScale = Vector3.one;
            _headTween = _headRect.DOScale(new Vector3(1f + _bobScale, 1f - _bobScale, 1f), _bobInterval)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }

        private void StopHeadBob()
        {
            _headTween?.Kill();
            _headTween = null;
            if (_headRect != null) _headRect.localScale = Vector3.one;
        }

        private async UniTask AskName()
        {
            if (_nameRoot == null || _nameInput == null) return;
            _nameRoot.SetActive(true);
            _nameInput.text = string.Empty;
            _nameInput.ActivateInputField();
            _nameInput.Select();

            await UniTask.WaitUntil(() =>
            {
                var kb = Keyboard.current;
                bool submit = kb != null && (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame);
                return submit && !string.IsNullOrWhiteSpace(_nameInput.text);
            });

            string name = _nameInput.text.Trim();
            G.PlayerName = name;
            PlayerPrefs.SetString(PlayerNameKey, name);
            PlayerPrefs.Save();

            _nameRoot.SetActive(false);
            await UniTask.NextFrame();
        }

        private static bool Pressed()
        {
            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame) return true;
            var kb = Keyboard.current;
            if (kb != null && (kb.spaceKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame)) return true;
            return false;
        }

        private void SetHidden(bool hidden)
        {
            if (_hideWhilePlaying == null) return;
            foreach (var go in _hideWhilePlaying)
                if (go != null) go.SetActive(!hidden);
        }
    }
}
