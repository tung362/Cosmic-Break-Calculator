using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI options editor for toggles
    /// </summary>
    public class OptionsToggleEdit : MonoBehaviour
    {
        /*Enums*/
        public enum OptionType { AudioVisualizer, VideoBackground, Grayscale };

        /*Configuration*/
        public Toggle ToggleBind;
        public OptionType OptionField;

        /*Cache*/
        private bool IgnoreSave = false;

        void Start()
        {
            Load();
        }

        public void SetValue(bool toggle)
        {
            switch(OptionField)
            {
                case OptionType.AudioVisualizer:
                    Calculator.instance.Settings.AudioVisualizer = toggle;
                    break;
                case OptionType.VideoBackground:
                    Calculator.instance.Settings.VideoBackground = toggle;
                    if (toggle)
                    {
                        if(!Calculator.instance.YoutubePlayer.IsPlaying()) Calculator.instance.YoutubePlayer.Play();
                    }
                    else Calculator.instance.YoutubePlayer.Stop();
                    break;
                case OptionType.Grayscale:
                    Calculator.instance.Settings.Grayscale = toggle;
                    if (toggle)
                    {
                        Calculator.instance.VideoImage.material.SetInt("_UseGrayscale", 1);
                        Calculator.instance.VideoImage.color = Calculator.instance.GrayscaleColor;
                    }
                    else
                    {
                        Calculator.instance.VideoImage.material.SetInt("_UseGrayscale", 0);
                        Calculator.instance.VideoImage.color = Color.white;
                    }
                    break;
                default:
                    break;
            }
            ToggleBind.SetIsOnWithoutNotify(toggle);
            Save();
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
                case OptionType.AudioVisualizer:
                    SetValue(Calculator.instance.Settings.AudioVisualizer);
                    break;
                case OptionType.VideoBackground:
                    SetValue(Calculator.instance.Settings.VideoBackground);
                    break;
                case OptionType.Grayscale:
                    SetValue(Calculator.instance.Settings.Grayscale);
                    break;
                default:
                    break;
            }
            IgnoreSave = false;
        }
    }
}
