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
    /// Pool item for outline directory
    /// </summary>
    public class OutlinePoolItem : MonoBehaviour, IPool<string>
    {
        public RectTransform PoolTransform;
        public Toggle FolderToggle;
        public Button FileButton;
        public Image IconImage;
        public TMP_Text NameText;
        public RectTransform ChildLine;
        public Sprite FolderIconOn;
        public Sprite FolderIconOff;
        public Color FolderColor;
        public Sprite FileIcon;
        public Color FileColor;

        public bool Enabled = true;

        public void SetActive(bool toggle)
        {
            if (Enabled == toggle) return;

            gameObject.SetActive(toggle);
            Enabled = toggle;
        }

        public void UpdateItem(string path)
        {

        }

        public void UpdateItem(string path, bool isFolder, bool flagged)
        {
            FolderToggle.gameObject.SetActive(isFolder);
            FileButton.gameObject.SetActive(!isFolder);
            ChildLine.gameObject.SetActive(!isFolder);
            if (isFolder)
            {
                IconImage.color = FolderColor;
                IconImage.sprite = flagged ? FolderIconOn : FolderIconOff;
                FolderToggle.SetIsOnWithoutNotify(flagged);
                NameText.text = new DirectoryInfo(path).Name;

                if (path == Path.GetFullPath(Calculator.PartPath) ||
                    path == Path.GetFullPath(Calculator.TunePath) ||
                    path == Path.GetFullPath(Calculator.CartridgePath))
                {
                    NameText.text = "Data Bundle";
                }

                if (path == Path.GetFullPath(Calculator.CustomContraptionPath) ||
                    path == Path.GetFullPath(Calculator.CustomPartPath) ||
                    path == Path.GetFullPath(Calculator.CustomTunePath) ||
                    path == Path.GetFullPath(Calculator.CustomCartridgePath))
                {
                    NameText.text = "Custom Data Bundle";
                }
            }
            else
            {
                IconImage.sprite = FileIcon;
                IconImage.color = FileColor;
                FileButton.interactable = flagged;
                NameText.text = Path.GetFileNameWithoutExtension(path);
            }
            UpdateItem(path);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
