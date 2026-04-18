using System.Collections.Generic;
using Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buffs
{
    public class RewardView : ViewBase
    {
        public List<Button> Buttons;

        public void Show(List<IAction> buffs)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (i < buffs.Count)
                {
                    var buff = buffs[i];
                    Buttons[i].gameObject.SetActive(true);
                    Buttons[i].GetComponentInChildren<TMP_Text>().text = buff.Name;
                    Buttons[i].onClick.RemoveAllListeners();
                    Buttons[i].onClick.AddListener(() => Pick(buff));
                }
                else
                {
                    Buttons[i].gameObject.SetActive(false);
                }
            }
        }

        private void Pick(IAction buff)
        {
            buff.Execute();
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            Debug.Log(G.Player.State);
        }
    }
}
