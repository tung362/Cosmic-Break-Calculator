using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI options editor for sliders
    /// </summary>
    public class OptionsSliderEdit : MonoBehaviour
    {
        /*Enums*/
        public enum OptionType { ScaleFactor, Sound };

        /*Configuration*/
        public Slider SliderBind;
        public TMP_InputField InputFieldBind;
        public OptionType OptionField;

        /*Cache*/
        private bool IgnoreSave = false;

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
            if (IgnoreSave) return;

            Calculator.instance.SaveOptions();
        }

        public void Load()
        {
            IgnoreSave = true;
            switch (OptionField)
            {
                case OptionType.ScaleFactor:
                    SetValue(Calculator.instance.Settings.ScaleFactor);
                    break;
                case OptionType.Sound:
                    SetValue(Calculator.instance.Settings.Sound);
                    break;
                default:
                    break;
            }
            IgnoreSave = false;
        }

        public void UpdateValue(float num)
        {
            switch (OptionField)
            {
                case OptionType.ScaleFactor:
                    Calculator.instance.Settings.ScaleFactor = num;
                    break;
                case OptionType.Sound:
                    Calculator.instance.Settings.Sound = num;
                    break;
                default:
                    break;
            }
            SliderBind.SetValueWithoutNotify(num);
            InputFieldBind.SetTextWithoutNotify(num.ToString());
            Save();
        }
    }
}
