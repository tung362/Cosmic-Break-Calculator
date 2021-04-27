using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view pool item for builds, parts, tunes, and cartridges
    /// </summary>
    public class ListViewAllPoolItem : IPool<string>
    {
        public Toggle SelectToggle;
        public Button BackgroundButton;
        public TMP_Text NameText;
        public TMP_Text LocationText;
        public TMP_Text DateText;
        public bool Enabled = true;

        public ListViewAllPoolItem(List<RectTransform> itemFields)
        {
            SelectToggle = itemFields[0].GetComponent<Toggle>();
            BackgroundButton = itemFields[1].GetComponent<Button>();
            NameText = itemFields[2].GetComponent<TMP_Text>();
            LocationText = itemFields[3].GetComponent<TMP_Text>();
            DateText = itemFields[4].GetComponent<TMP_Text>();
        }

        public void SetActive(bool toggle)
        {
            if (Enabled == toggle) return;

            SelectToggle.gameObject.SetActive(toggle);
            BackgroundButton.gameObject.SetActive(toggle);
            NameText.gameObject.SetActive(toggle);
            LocationText.gameObject.SetActive(toggle);
            DateText.gameObject.SetActive(toggle);
            Enabled = toggle;
        }

        public void UpdateItem(string path)
        {
            NameText.text = Path.GetFileNameWithoutExtension(path);
            LocationText.text = path;
            DateText.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
        }

        public void UpdateItem(string path, bool selected)
        {
            SelectToggle.isOn = selected;
            UpdateItem(path);
        }

        public void Destroy()
        {
            Object.Destroy(SelectToggle.gameObject);
            Object.Destroy(BackgroundButton.gameObject);
            Object.Destroy(NameText.gameObject);
            Object.Destroy(LocationText.gameObject);
            Object.Destroy(DateText.gameObject);
        }
    }
}
