using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a outline file
    /// </summary>
    public class FileItemSlot : MonoBehaviour, IDirectoryFile
    {
        public string Path { get; set; }
        public IDirectoryFile Parent { get; set; }
        public int BranchCount { get; set; } = 1;
        public int BranchIndex { get; set; }

        [Header("Binds")]
        public TMP_Text NameText;
        public Button SelectButton;
    }
}
