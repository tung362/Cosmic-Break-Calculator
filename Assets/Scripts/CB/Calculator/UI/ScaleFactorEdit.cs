using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI scale factor editor
    /// </summary>
    public class ScaleFactorEdit : MonoBehaviour
    {
        public Slider ScaleFactorSlider;
        public TMP_InputField ScaleFactorInputField;

        void Start()
        {
            Load();
        }

        public void SetValue(float num)
        {
            UpdateValue(num);
        }

        public void SetValue(string text)
        {
            float.TryParse(text, out float num);
            UpdateValue(num);
        }

        public void Save()
        {
            Calculator.instance.SaveOptions();
        }

        public void Load()
        {
            SetValue(Calculator.instance.Settings.ScaleFactor);
        }

        public void UpdateValue(float num)
        {
            Calculator.instance.Settings.ScaleFactor = num;
            ScaleFactorSlider.SetValueWithoutNotify(num);
            ScaleFactorInputField.SetTextWithoutNotify(num.ToString());
            Save();
        }
    }
}
