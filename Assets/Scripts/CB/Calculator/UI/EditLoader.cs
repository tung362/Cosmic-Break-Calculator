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
        public TMP_Dropdown PartBDTypeBind;
        public TMP_Dropdown PartSizeBind;
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
        public TMP_InputField DiscriptionBind;
        public TMP_InputField SlotsBind;

        void OnEnable()
        {
            //Set listener
            Builder.OnRedraw += OnRedraw;
        }

        void OnDisable()
        {
            //Unset listener
            Builder.OnRedraw -= OnRedraw;
        }

        #region Listeners
        void OnRedraw(PartJointSlot jointSlot)
        {
            Builder.IgnoreToggles = true;
            //Joint
            JointFixedBind.isOn = jointSlot.Slot.Fixed;
            JointRequiredBind.isOn = jointSlot.Slot.Required;
            JointTagsBind.SetTextWithoutNotify(string.Join(", ", jointSlot.Slot.Tags.Values));
            JointTypeBind.SetValueWithoutNotify((int)jointSlot.Slot.Joint);
            //Part
            CreatePartBind.isOn = jointSlot.Slot.EquipedPart != null ? true : false;
            if (jointSlot.Slot.EquipedPart != null)
            {
                PartNameBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.Name);
                PartTagsBind.SetTextWithoutNotify(string.Join(", ", jointSlot.Slot.EquipedPart.Tags.Values));
                if(jointSlot.transform == Builder.Root.transform && jointSlot.Slot.EquipedPart.Joint == PartJoint.JointType.BD)
                {
                    PartBDTypeBind.transform.parent.gameObject.SetActive(true);
                    PartBDMaskBind.transform.parent.gameObject.SetActive(false);
                    for(int i = 0; i < 4; i++)
                    {
                        if(jointSlot.Slot.EquipedPart.BDMask.HasFlag(i))
                        {
                            PartBDTypeBind.SetValueWithoutNotify(i);
                            return;
                        }
                    }
                }
                else
                {
                    PartBDMaskBind.transform.parent.gameObject.SetActive(true);
                    PartBDTypeBind.transform.parent.gameObject.SetActive(false);
                    PartBDMaskBind.SetValueNoCallback(jointSlot.Slot.EquipedPart.BDMask);
                }
                PartSizeBind.SetValueWithoutNotify((int)jointSlot.Slot.EquipedPart.Size);
                PartCostBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.COST.ToString());
                PartHPBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.HP.ToString());
                PartSTRBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.STR.ToString());
                PartTECBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.TEC.ToString());
                PartWLKBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.WLK.ToString());
                PartFLYBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.FLY.ToString());
                PartTGHBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.TGH.ToString());
                PartCAPABind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.BaseStats.CAPA.ToString());
                //Main weapon
                MainBind.isOn = jointSlot.Slot.EquipedPart.MainWeapon != null ? true : false;
                if(jointSlot.Slot.EquipedPart.MainWeapon != null)
                {
                    MainForceBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.MainWeapon.Force.ToString());
                    MainAmmoBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.MainWeapon.Ammo.ToString());
                    MainRangeBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.MainWeapon.Range.ToString());
                    MainSpeedBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.MainWeapon.Speed.ToString());
                    MainIntBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.MainWeapon.Int.ToString());
                    UpdateMainDamage();
                }
                //Sub weapon
                SubBind.isOn = jointSlot.Slot.EquipedPart.SubWeapon != null ? true : false;
                if (jointSlot.Slot.EquipedPart.SubWeapon != null)
                {
                    SubForceBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.SubWeapon.Force.ToString());
                    SubAmmoBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.SubWeapon.Ammo.ToString());
                    SubRangeBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.SubWeapon.Range.ToString());
                    SubSpeedBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.SubWeapon.Speed.ToString());
                    SubIntBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.SubWeapon.Int.ToString());
                    UpdateSubDamage();
                }
                DiscriptionBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.Description);
                SlotsBind.SetTextWithoutNotify(jointSlot.Slot.EquipedPart.Tunes.Count.ToString());
            }
            Builder.IgnoreToggles = false;
        }
        #endregion

        #region Utils
        public void UpdateMainDamage()
        {
            int.TryParse(MainForceBind.text, out int force);
            int.TryParse(PartTECBind.text, out int stat);
            MainDMGBind.text = WeaponStats.CalculateDamage(force, stat, true).ToString();
        }

        public void UpdateSubDamage()
        {
            int.TryParse(SubForceBind.text, out int force);
            int.TryParse(PartSTRBind.text, out int stat);
            SubDMGBind.text = WeaponStats.CalculateDamage(force, stat, false).ToString();
        }
        #endregion
    }
}
