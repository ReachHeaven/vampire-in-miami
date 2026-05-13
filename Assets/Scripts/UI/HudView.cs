using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("Health")]     private TMP_Text _health;
        [SerializeField, FormerlySerializedAs("Message")]    private TMP_Text _message;
        [SerializeField, FormerlySerializedAs("Wave")]       private TMP_Text _wave;
        [SerializeField, FormerlySerializedAs("Level")]      private TMP_Text _level;
        [SerializeField, FormerlySerializedAs("Experience")] private TMP_Text _experience;

        public void SetMessage(string msg) => _message.SetText(msg);

        public void ClearMessage() => _message.SetText("");

        public void SetHealth(int total, int current) =>
            _health.text = $"Health: {current}/{total}";

        public void SetLevel(int current) =>
            _level.text = $"Level: {current}";

        public void SetExperience(int current, int total) =>
            _experience.text = $"Experience: {current}/{total}";

        public void SetWave(string current, string total) =>
            _wave.text = $"Wave: {current}/{total}";
    }
}
