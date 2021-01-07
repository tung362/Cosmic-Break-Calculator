using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI sound volume editor
    /// </summary>
    public class SoundEdit : MonoBehaviour
    {
        public Slider AudioSlider;
        public TMP_InputField AudioInputField;

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
            SetValue(Calculator.instance.Settings.Sound);
        }

        public void UpdateValue(float num)
        {
            Calculator.instance.Settings.Sound = num;
            AudioSlider.SetValueWithoutNotify(num);
            AudioInputField.SetTextWithoutNotify(num.ToString());
            Save();
        }
    }
}
