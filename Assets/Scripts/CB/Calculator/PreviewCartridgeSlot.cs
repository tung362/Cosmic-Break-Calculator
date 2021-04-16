using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a preview cartridge
    /// </summary>
    public class PreviewCartridgeSlot : MonoBehaviour
    {
        [Header("Binds")]
        public Image BackgroundImage;
        public TMP_InputField TitleInputField;
        public TMP_InputField CostInputField;
    }
}
