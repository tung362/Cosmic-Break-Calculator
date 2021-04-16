using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI Bind for handling displays of hints
    /// </summary>
    public class UIHintBind : MonoBehaviour
    {
        public static UIHintBind instance { get; private set; }

        public TMP_InputField Bind;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.LogWarning("Warning! Multiple instances of \"UIHintBind\"");
        }

        #region Utils
        public void UpdateText(string text)
        {
            Bind.text = text;
        }
        #endregion
    }
}
