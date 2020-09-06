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
        [Header("Part Edit Binds")]
        public RectTransform PartEditSection;
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
        public Toggle AllowJBind;
        public Toggle IsJBind;
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
            if(!jointSlot)
            {
                PartEditSection.gameObject.SetActive(false);
                return;
            }
            PartEditSection.gameObject.SetActive(true);

            Builder.IgnoreEvents = true;
            //Joint
            JointFixedBind.isOn = jointSlot.Slot.Fixed;
            JointRequiredBind.isOn = jointSlot.Slot.Required;
            JointTagsBind.text = string.Join(", ", jointSlot.Slot.Tags.Values);
            JointTypeBind.value = (int)jointSlot.Slot.Joint;
            //Part
            CreatePartBind.isOn = jointSlot.Slot.EquipedPart != null ? true : false;
            if (jointSlot.Slot.EquipedPart != null)
            {
                PartNameBind.text = jointSlot.Slot.EquipedPart.Name;
                PartTagsBind.text = string.Join(", ", jointSlot.Slot.EquipedPart.Tags.Values);
                if(jointSlot.transform == Builder.Root.transform && jointSlot.Slot.EquipedPart.Joint == PartJoint.JointType.BD)
                {
                    PartBDTypeBind.transform.parent.gameObject.SetActive(true);
                    PartBDMaskBind.transform.parent.gameObject.SetActive(false);
                    for(int i = 0; i < 4; i++)
                    {
                        if(jointSlot.Slot.EquipedPart.BDMask.HasFlag(i))
                        {
                            PartBDTypeBind.value = i;
                            break;
                        }
                    }
                }
                else
                {
                    PartBDMaskBind.transform.parent.gameObject.SetActive(true);
                    PartBDTypeBind.transform.parent.gameObject.SetActive(false);
                    PartBDMaskBind.Value = jointSlot.Slot.EquipedPart.BDMask;
                }
                PartSizeBind.value = (int)jointSlot.Slot.EquipedPart.Size;
                PartCostBind.text = jointSlot.Slot.EquipedPart.BaseStats.COST.ToString();
                PartHPBind.text = jointSlot.Slot.EquipedPart.BaseStats.HP.ToString();
                PartSTRBind.text = jointSlot.Slot.EquipedPart.BaseStats.STR.ToString();
                PartTECBind.text = jointSlot.Slot.EquipedPart.BaseStats.TEC.ToString();
                PartWLKBind.text = jointSlot.Slot.EquipedPart.BaseStats.WLK.ToString();
                PartFLYBind.text = jointSlot.Slot.EquipedPart.BaseStats.FLY.ToString();
                PartTGHBind.text = jointSlot.Slot.EquipedPart.BaseStats.TGH.ToString();
                PartCAPABind.text = jointSlot.Slot.EquipedPart.BaseStats.CAPA.ToString();
                //Main weapon
                MainBind.isOn = jointSlot.Slot.EquipedPart.MainWeapon != null ? true : false;
                if(jointSlot.Slot.EquipedPart.MainWeapon != null)
                {
                    MainForceBind.text = jointSlot.Slot.EquipedPart.MainWeapon.Force.ToString();
                    MainAmmoBind.text = jointSlot.Slot.EquipedPart.MainWeapon.Ammo.ToString();
                    MainRangeBind.text = jointSlot.Slot.EquipedPart.MainWeapon.Range.ToString();
                    MainSpeedBind.text = jointSlot.Slot.EquipedPart.MainWeapon.Speed.ToString();
                    MainIntBind.text = jointSlot.Slot.EquipedPart.MainWeapon.Int.ToString();
                    UpdateMainDamage();
                }
                //Sub weapon
                SubBind.isOn = jointSlot.Slot.EquipedPart.SubWeapon != null ? true : false;
                if (jointSlot.Slot.EquipedPart.SubWeapon != null)
                {
                    SubForceBind.text = jointSlot.Slot.EquipedPart.SubWeapon.Force.ToString();
                    SubAmmoBind.text = jointSlot.Slot.EquipedPart.SubWeapon.Ammo.ToString();
                    SubRangeBind.text = jointSlot.Slot.EquipedPart.SubWeapon.Range.ToString();
                    SubSpeedBind.text = jointSlot.Slot.EquipedPart.SubWeapon.Speed.ToString();
                    SubIntBind.text = jointSlot.Slot.EquipedPart.SubWeapon.Int.ToString();
                    UpdateSubDamage();
                }
                DiscriptionBind.text = jointSlot.Slot.EquipedPart.Description;
                AllowJBind.isOn = jointSlot.Slot.EquipedPart.AllowJs;
                IsJBind.isOn = jointSlot.Slot.EquipedPart.IsJ;
                SlotsBind.text = jointSlot.Slot.EquipedPart.Tunes.Count.ToString();
            }
            Builder.IgnoreEvents = false;
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
