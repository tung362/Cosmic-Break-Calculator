using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.UI
{
    /// <summary>
    /// Switches text color when threshold are met in input fields
    /// </summary>
    public class UINumberColor : MonoBehaviour
    {
        public TMP_InputField Bind;
        public float Threshold;
        public Color Color1;
        public Color Color2;

        public void UpdateColor(string text)
        {
            float.TryParse(text, out float value);
            if (value < Threshold) Bind.textComponent.color = Color1;
            else Bind.textComponent.color = Color2;
        }
    }
}
