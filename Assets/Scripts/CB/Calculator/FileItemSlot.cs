using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a outline file
    /// </summary>
    public class FileItemSlot : MonoBehaviour, IDirectoryFile
    {
        public string Path { get; set; }

        [Header("Binds")]
        public TMP_Text NameText;

        [Header("Builder")]
        public IDirectoryFile Parent;
        public int BranchCount = 1;
        public int BranchIndex;
    }
}
