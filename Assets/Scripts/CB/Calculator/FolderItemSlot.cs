using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a outline folder
    /// </summary>
    public class FolderItemSlot : MonoBehaviour, IDirectoryFile
    {
        public string Path { get; set; }
        public IDirectoryFile Parent { get; set; }
        public int BranchCount { get; set; } = 1;
        public int BranchIndex { get; set; }

        [Header("Binds")]
        public TMP_Text NameText;
        public Toggle ShowHideToggle;
        public RectTransform FileSlotOrigin;
        public RectTransform ChildLine;

        [Header("Builder")]
        public OutlineBuilder Builder;
        public List<IDirectoryFile> SubItems = new List<IDirectoryFile>();
        public bool Show = true;

        public void ShowHide(bool toggle)
        {
            Show = toggle;
            if (toggle) Builder.Show(Path);
            else Builder.Hide(Path);
            Builder.RecalculateContent(Builder.Template);
        }
    }
}
