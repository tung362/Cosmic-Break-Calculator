using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a preview craft material
    /// </summary>
    public class PreviewCraftMaterialSlot : MonoBehaviour
    {
        [Header("Binds")]
        public TMP_InputField NameInputField;
        public TMP_InputField AmountInputField;
    }
}
