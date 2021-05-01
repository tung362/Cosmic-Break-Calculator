using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;
using CB.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of option settings
    /// </summary>
    public class OptionsBuilder : MonoBehaviour
    {
        [Header("Sliders Fields")]
        public Slider ScaleFactorSlider;
        public TMP_InputField ScaleFactorInputField;
        public Slider SoundSlider;
        public TMP_InputField SoundInputField;

        [Header("Toggles")]
        public Toggle AudioVisualizerToggle;
        public Toggle VideoBackgroundToggle;
        public Toggle GrayscaleToggle;

        [Header("Input Fields")]
        public TMP_InputField LoadChunkInputField;

        [Header("Dropdowns")]
        public TMP_Dropdown LanguageDropdown;

        /*Cache*/
        private bool IgnoreSave = false;

        void Start()
        {
            Load();
        }

        #region Creation And Removal
        public void Save()
        {
            if (IgnoreSave) return;
            Calculator.instance.SaveOptions();
        }

        public void Load()
        {
            IgnoreSave = true;
            UpdateScaleFactor(Calculator.instance.Settings.ScaleFactor);
            UpdateSound(Calculator.instance.Settings.Sound);
            UpdateAudioVisualizer(Calculator.instance.Settings.AudioVisualizer);
            UpdateVideoBackground(Calculator.instance.Settings.VideoBackground);
            UpdateGrayscale(Calculator.instance.Settings.Grayscale);
            UpdateLoadChunk(Calculator.instance.Settings.FilesLoadedPerChunk);
            UpdateLanguage((int)Calculator.instance.Settings.LanguageMode);
            IgnoreSave = false;
        }
        #endregion

        #region Editing
        /*Sliders*/
        public void UpdateScaleFactor(string text)
        {
            float.TryParse(text, out float num);
            UpdateScaleFactor(num);
        }

        public void UpdateScaleFactor(float num)
        {
            Calculator.instance.Settings.ScaleFactor = num;
            Calculator.instance.RootCanvas.GetComponent<CanvasScaler>().scaleFactor = Calculator.instance.Settings.ScaleFactor;
            WindowResizeEvent.instance.RequestOnWindowResize();
            ScaleFactorSlider.SetValueWithoutNotify(num);
            ScaleFactorInputField.SetTextWithoutNotify(num.ToString());
            Save();
        }

        public void UpdateSound(string text)
        {
            float.TryParse(text, out float num);
            UpdateSound(num);
        }

        public void UpdateSound(float num)
        {
            Calculator.instance.Settings.Sound = num;
            Calculator.instance.YoutubePlayer.Volume(Calculator.instance.Settings.Sound);
            SoundSlider.SetValueWithoutNotify(num);
            SoundInputField.SetTextWithoutNotify(num.ToString());
            Save();
        }

        /*Toggles*/
        public void UpdateAudioVisualizer(bool toggle)
        {
            Calculator.instance.Settings.AudioVisualizer = toggle;
            AudioVisualizerToggle.SetIsOnWithoutNotify(toggle);
            Save();
        }

        public void UpdateVideoBackground(bool toggle)
        {
            Calculator.instance.Settings.VideoBackground = toggle;
            if (toggle)
            {
                if (!Calculator.instance.YoutubePlayer.IsPlaying()) Calculator.instance.YoutubePlayer.Play();
            }
            else Calculator.instance.YoutubePlayer.Stop();
            VideoBackgroundToggle.SetIsOnWithoutNotify(toggle);
            Save();
        }

        public void UpdateGrayscale(bool toggle)
        {
            Calculator.instance.Settings.Grayscale = toggle;
            if (toggle)
            {
                Calculator.instance.VideoPlayerMaterial.SetInt("_UseGrayscale", 1);
                Calculator.instance.VideoPlayerMaterial.SetColor("_VideoColor", Calculator.instance.GrayscaleColor);
            }
            else
            {
                Calculator.instance.VideoPlayerMaterial.SetInt("_UseGrayscale", 0);
                Calculator.instance.VideoPlayerMaterial.SetColor("_VideoColor", Color.white);
            }
            GrayscaleToggle.SetIsOnWithoutNotify(toggle);
            Save();
        }

        /*Input Fields*/
        public void UpdateLoadChunk(string text)
        {
            int.TryParse(text, out int num);
            UpdateLoadChunk(num);
        }

        public void UpdateLoadChunk(int num)
        {
            Calculator.instance.Settings.FilesLoadedPerChunk = num;
            LoadChunkInputField.SetTextWithoutNotify(num.ToString());
            Save();
        }

        /*Dropdowns*/
        public void UpdateLanguage(int num)
        {
            Calculator.instance.Settings.LanguageMode = (Options.LanguageType)num;
            LanguageDropdown.SetValueWithoutNotify(num);
            Save();
        }
        #endregion
    }
}
