using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI video background toggle editor
    /// </summary>
    public class VideoBGEdit : MonoBehaviour
    {
        public Toggle VideoBGToggle;

        void Start()
        {
            Load();
        }

        public void SetValue(bool toggle)
        {
            Calculator.instance.Settings.VideoBackground = toggle;
            VideoBGToggle.SetIsOnWithoutNotify(toggle);
            Save();
        }

        public void Save()
        {
            Calculator.instance.SaveOptions();
        }

        public void Load()
        {
            SetValue(Calculator.instance.Settings.VideoBackground);
        }
    }
}
