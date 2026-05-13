using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MenuView : ModalViewBase
    {
        public enum ItemKind { Start, Quit }

        [Serializable]
        public class MenuItem
        {
            public ItemKind Kind;
            public string Text;
            public TextMeshProUGUI Label;
        }

        [SerializeField] private MenuItem[] _items;
        [SerializeField] private Color _normal = new Color(1f, 1f, 1f, 0.55f);
        [SerializeField] private Color _selected = Color.white;
        [SerializeField] private string _selectedPrefix = "> ";

        private int _index;
        private bool _shown;

        public void Show()
        {
            if (_skip || _root == null || _items == null || _items.Length == 0) return;
            if (_shown) return;
            _shown = true;

            ShowRoot();
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

            await FadeRoot(0f);
            FinishHide(chainNext);
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
    }
}
