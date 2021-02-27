using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of CraftMaterial
    /// </summary>
    public class CraftMaterialSlot : MonoBehaviour
    {
        [Header("Binds")]
        public TMP_InputField NameInputField;
        public TMP_InputField AmountInputField;
        public Button RemoveButton;

        [Header("Builder")]
        public TuneBuilder Builder;

        public void UpdateMaterialName(string text)
        {
            Builder.UpdateMaterialName(this, text);
        }

        public void UpdateMaterialAmount(string text)
        {
            Builder.UpdateMaterialAmount(this, text);
        }
    }
}
