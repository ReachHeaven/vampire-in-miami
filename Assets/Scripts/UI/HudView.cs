using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HudView : MonoBehaviour
    {
        public TMP_Text Health;
        public TMP_Text Message;
        public TMP_Text Wave;
        public TMP_Text Level;
        public TMP_Text Experience;

        public void SetMessage(string msg) => Message.SetText(msg);
        
        public void ClearMessage() => Message.SetText("");

        public void SetHealth(int total, int current)
        {
            Health.text = $"Health: {current}/{total}";
        }
        public void SetLevel(int current)
        {
            Debug.Log($"Level: {current}");
            Level.text = $"Level: {current}";
        }
        public void SetExperience(int current, int total)
        {
            Debug.Log( $"Experience: {current}/{total}");
            Experience.text = $"Experience: {current}/{total}";
        }

        public void SetWave(string current, string total)
        {
            Wave.text = $"Wave: {current}/{total}";
        }
    }
}