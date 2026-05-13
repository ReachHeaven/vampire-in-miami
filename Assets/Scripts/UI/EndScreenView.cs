using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenView : ModalViewBase
    {
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _hint;
        [SerializeField] private string _victoryTitle = "VICTORY";
        [SerializeField] private string _defeatTitle = "GAME OVER";
        [SerializeField] private string _hintText = "Press any key to restart";
        [SerializeField] private Color _victoryColor = Color.white;
        [SerializeField] private Color _defeatColor = new(1f, 0.35f, 0.35f);
        [SerializeField] private Color _victoryBackground = new(0.05f, 0.1f, 0.15f, 0.85f);
        [SerializeField] private Color _defeatBackground = new(0.15f, 0.02f, 0.02f, 0.85f);
        [SerializeField] private float _delayBeforeInput = 0.6f;

        private bool _shown;

        public async UniTask Show(bool victory)
        {
            if (_shown || _root == null) return;
            _shown = true;

            if (_title != null)
            {
                _title.text = victory ? _victoryTitle : _defeatTitle;
                _title.color = victory ? _victoryColor : _defeatColor;
            }
            if (_hint != null) _hint.text = _hintText;
            if (_background != null)
                _background.color = victory ? _victoryBackground : _defeatBackground;

            ShowRoot(alpha: 0f);
            await FadeRoot(1f);

            await UniTask.WaitForSeconds(_delayBeforeInput, ignoreTimeScale: true);
            await UniTask.WaitUntil(AnyKeyPressed);

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
