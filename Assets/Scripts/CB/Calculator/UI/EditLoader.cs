using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// UI loader for joints and parts editor
    /// </summary>
    public class EditLoader : MonoBehaviour
    {
        public PartBuilder Builder;

        /*Binds*/
        //Joint
        public Toggle JointFixedBind;
        public Toggle JointRequiredBind;
        public TMP_InputField JointTagsBind;
        public TMP_Dropdown JointTypeBind;
        public Toggle CreatePartBind;
        //Part
        public TMP_InputField PartNameBind;
        public TMP_InputField PartTagsBind;
        public UIMaskDropdown PartBDMaskBind;
        public Dropdown PartSizeBind;
        public TMP_InputField PartCostBind;
        public TMP_InputField PartHPBind;
        public TMP_InputField PartSTRBind;
        public TMP_InputField PartTECBind;
        public TMP_InputField PartWLKBind;
        public TMP_InputField PartFLYBind;
        public TMP_InputField PartTGHBind;
        public TMP_InputField PartCAPABind;
        public Toggle MainBind;
        public TMP_InputField MainForceBind;
        public TMP_InputField MainAmmoBind;
        public TMP_InputField MainRangeBind;
        public TMP_InputField MainSpeedBind;
        public TMP_InputField MainIntBind;
        public TMP_InputField MainDMGBind;
        public Toggle SubBind;
        public TMP_InputField SubForceBind;
        public TMP_InputField SubAmmoBind;
        public TMP_InputField SubRangeBind;
        public TMP_InputField SubSpeedBind;
        public TMP_InputField SubIntBind;
        public TMP_InputField SubDMGBind;

        void OnEnable()
        {
            //Set listener
            Builder.OnJointSelect += OnJointSelect;
        }

        void OnDisable()
        {
            //Unset listener
            Builder.OnJointSelect -= OnJointSelect;
        }

        #region Listeners
        void OnJointSelect(PartJointSlot jointSlot)
        {
            JointFixedBind.SetIsOnWithoutNotify(jointSlot.Slot.Editable);

            if(jointSlot.Slot.EquipedPart != null)
            {

            }

            //Load for multiple joints
            for (int i = 0; i < Builder.Slots.Count; i++)
            {

            }
        }
        #endregion

        #region Utils
        public void UpdateDMG(bool isMain)
        {
            int.TryParse(MainForceBind.text, out int force);
            int.TryParse(isMain ? PartTECBind.text : PartSTRBind.text, out int stat);
            WeaponStats.CalculateDamage(force, stat, isMain);
        }
        #endregion
    }
}
