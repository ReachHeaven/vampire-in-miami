using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MenuView : MonoBehaviour
    {
        public enum ItemKind { Start, Quit }

        [Serializable]
        public class MenuItem
        {
            public ItemKind Kind;
            public string Text;
            public TextMeshProUGUI Label;
        }

        [SerializeField] private bool _skip;
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private MenuItem[] _items;
        [SerializeField] private Color _normal = new Color(1f, 1f, 1f, 0.55f);
        [SerializeField] private Color _selected = Color.white;
        [SerializeField] private string _selectedPrefix = "> ";
        [SerializeField] private float _rootFade = 0.4f;
        [SerializeField] private GameObject[] _hideWhilePlaying;

        private int _index;
        private bool _shown;

        private void Awake()
        {
            if (_root != null) _root.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (_skip || _root == null || _items == null || _items.Length == 0) return;
            if (_shown) return;
            _shown = true;

            SetHidden(true);
            Time.timeScale = 0f;

            _root.gameObject.SetActive(true);
            _root.alpha = 1f;

            _index = 0;
            Refresh();
        }

        public async UniTask Play(bool chainNext = false)
        {
            if (_skip || _root == null || _items == null || _items.Length == 0)
            {
                if (_root != null) _root.gameObject.SetActive(false);
                return;
            }

            Show();

            ItemKind chosen = ItemKind.Start;
            while (true)
            {
                var kb = Keyboard.current;
                if (kb != null)
                {
                    if (kb.upArrowKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame)
                    {
                        _index = (_index - 1 + _items.Length) % _items.Length;
                        Refresh();
                    }
                    else if (kb.downArrowKey.wasPressedThisFrame || kb.sKey.wasPressedThisFrame)
                    {
                        _index = (_index + 1) % _items.Length;
                        Refresh();
                    }
                    else if (kb.enterKey.wasPressedThisFrame
                          || kb.numpadEnterKey.wasPressedThisFrame
                          || kb.spaceKey.wasPressedThisFrame)
                    {
                        chosen = _items[_index].Kind;
                        break;
                    }
                }
                await UniTask.Yield();
            }

            if (chosen == ItemKind.Quit)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                await UniTask.Never(CancellationToken.None);
                return;
            }

            await _root.DOFade(0f, _rootFade).SetEase(Ease.OutSine)
                .SetUpdate(true).AsyncWaitForCompletion();
            _root.gameObject.SetActive(false);

            if (!chainNext)
            {
                Time.timeScale = 1f;
                SetHidden(false);
            }
        }

        private void Refresh()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                var it = _items[i];
                if (it == null || it.Label == null) continue;
                bool selected = i == _index;
                it.Label.color = selected ? _selected : _normal;
                it.Label.text = (selected ? _selectedPrefix : string.Empty) + it.Text;
            }
        }

        private void SetHidden(bool hidden)
        {
            if (_hideWhilePlaying == null) return;
            foreach (var go in _hideWhilePlaying)
                if (go != null) go.SetActive(!hidden);
        }
    }
}
