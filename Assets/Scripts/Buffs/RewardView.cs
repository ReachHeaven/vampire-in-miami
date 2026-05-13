using System.Collections.Generic;
using Base;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Buffs
{
    public class RewardView : ViewBase
    {
        [SerializeField, FormerlySerializedAs("Buttons")] private List<Button> _buttons;

        public void Show(List<IAction> buffs)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;

            for (int i = 0; i < _buttons.Count; i++)
            {
                if (i < buffs.Count)
                {
                    var buff = buffs[i];
                    _buttons[i].gameObject.SetActive(true);
                    _buttons[i].GetComponentInChildren<TMP_Text>().text = buff.Name;
                    _buttons[i].onClick.RemoveAllListeners();
                    _buttons[i].onClick.AddListener(() => Pick(buff));
                }
                else
                {
                    _buttons[i].gameObject.SetActive(false);
                }
            }
        }

        private void Pick(IAction buff)
        {
            buff.Execute();
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
