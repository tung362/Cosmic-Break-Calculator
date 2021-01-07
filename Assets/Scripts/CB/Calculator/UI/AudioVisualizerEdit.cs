using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI audio visualizer toggle editor
    /// </summary>
    public class AudioVisualizerEdit : MonoBehaviour
    {
        public Toggle AudioVisualizerToggle;

        void Start()
        {
            Load();
        }

        public void SetValue(bool toggle)
        {
            Calculator.instance.Settings.AudioVisualizer = toggle;
            AudioVisualizerToggle.SetIsOnWithoutNotify(toggle);
            Save();
        }

        public void Save()
        {
            Calculator.instance.SaveOptions();
        }

        public void Load()
        {
            SetValue(Calculator.instance.Settings.AudioVisualizer);
        }
    }
}
