using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a video url
    /// </summary>
    public class VideoUrlSlot : MonoBehaviour
    {
        [Header("Binds")]
        public TMP_InputField UrlInputField;
        public Button RemoveButton;
    }
}
