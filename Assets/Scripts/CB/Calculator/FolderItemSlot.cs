using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a outline folder
    /// </summary>
    public class FolderItemSlot : MonoBehaviour, IDirectoryFile
    {
        public string Path { get; set; }

        [Header("Binds")]
        public TMP_Text NameText;
        public RectTransform FileSlotOrigin;

        [Header("Builder")]
        public IDirectoryFile Parent;
        public List<IDirectoryFile> SubItems = new List<IDirectoryFile>();
        public int BranchCount = 1;
        public int BranchIndex;
    }
}
