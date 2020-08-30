using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Handles displaying description on the UI
    /// </summary>
    public class UIDescriptionModule : MonoBehaviour
    {
        public interface IUIDescription
        {
            string GetDescription();
        }

        public static UIDescriptionModule instance { get; private set; }

        public List<TextMeshProUGUI> Binds;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"UIDescriptionModule\"");
        }

        #region Utils
        public void UpdateText(string text)
        {
            for(int i = 0; i < Binds.Count; i++) Binds[i].text = text;
        }
        #endregion
    }
}
