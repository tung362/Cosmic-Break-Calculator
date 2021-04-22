using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI loading screen for files
    /// </summary>
    public class LoadProgress : MonoBehaviour
    {
        public UIBar ProgressBar;
        public TMP_Text FileNameText;

        public void UpdateProgress(float progress, string path)
        {
            ProgressBar.UpdateProgress(progress);
            FileNameText.text = Path.GetFileNameWithoutExtension(path);
        }
    }
}
