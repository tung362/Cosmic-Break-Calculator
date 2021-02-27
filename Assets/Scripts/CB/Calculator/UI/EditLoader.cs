using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI loader for joints and parts editor
    /// </summary>
    public class EditLoader : MonoBehaviour
    {
        public PartBuilder Builder;
        public UIEnableSectionFitTrigger SectionFitter;

        /*Binds*/
        [Header("Part Edit Binds")]
        public RectTransform PartEditSection;
        //Joint header
        public Toggle JointFixedBind;
        public Toggle JointRequiredBind;
        public TMP_InputField JointTagsBind;
        public TMP_Dropdown JointTypeBind;
        public Toggle CreatePartBind;
        //Part header
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

        [Header("BD Edit Binds")]
        public RectTransform CartridgeHeaderRegion;
        public RectTransform LevelBGMask;
        public RectTransform CostTitleMask;
        public RectTransform CartridgeMask;
        //Robot Information header
        public TMP_InputField InfoTypeBind;
        public TMP_InputField InfoHPBind;
        public TMP_InputField InfoSizeBind;
        public TMP_InputField InfoCostBind;
        public TMP_InputField InfoCapaBind;
        public TMP_InputField InfoStrBind;
        public TMP_InputField InfoTecBind;
        public TMP_InputField InfoWlkBind;
        public TMP_InputField InfoFlyBind;
        public TMP_InputField InfoTghBind;
        public TMP_InputField InfoStrBarBind;
        public TMP_InputField InfoTecBarBind;
        public TMP_InputField InfoWlkBarBind;
        public TMP_InputField InfoFlyBarBind;
        public TMP_InputField InfoTghBarBind;
        //Cartridge header
        public TMP_InputField InfoLevelBind;

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
        void OnRedraw(ISelectable selectable)
        {
            if (Builder.Root.Slot.Joint != PartJoint.JointType.BD)
            {
                CartridgeHeaderRegion.gameObject.SetActive(false);
                LevelBGMask.gameObject.SetActive(false);
                CostTitleMask.gameObject.SetActive(false);
                CartridgeMask.gameObject.SetActive(false);
            }
            else
            {
                CartridgeHeaderRegion.gameObject.SetActive(true);
                LevelBGMask.gameObject.SetActive(true);
                CostTitleMask.gameObject.SetActive(true);
                CartridgeMask.gameObject.SetActive(true);
            }
            /*BD Edit Section*/
            UpdateInfoType();
            UpdateInfoHP();
            UpdateInfoSize();
            UpdateInfoCost();
            UpdateInfoCapa();
            UpdateInfoStr();
            UpdateInfoTec();
            UpdateInfoWlk();
            UpdateInfoFly();
            UpdateInfoTgh();
            UpdateInfoLevel();
            SectionFitter.Resize();

            if(selectable != null)
            {
                /*Part Edit Section*/
                //If the selectable type is a PartJointSlot
                if (selectable.GetType() == typeof(PartJointSlot))
                {
                    PartJointSlot jointSlot = (PartJointSlot)selectable;
                    PartEditSection.gameObject.SetActive(true);
                    Builder.IgnoreEvents = true;
                    //Joint
                    JointFixedBind.isOn = jointSlot.Slot.Fixed;
                    JointRequiredBind.isOn = jointSlot.Slot.Required;
                    AllowJBind.isOn = jointSlot.Slot.AllowJs;
                    JointTagsBind.text = string.Join(", ", jointSlot.Slot.Tags.Values);
                    JointTypeBind.value = (int)jointSlot.Slot.Joint;
                    //Part
                    CreatePartBind.isOn = jointSlot.Slot.EquipedPart != null ? true : false;
                    if (jointSlot.Slot.EquipedPart != null)
                    {
                        PartNameBind.text = jointSlot.Slot.EquipedPart.Name;
                        PartTagsBind.text = string.Join(", ", jointSlot.Slot.EquipedPart.Tags.Values);
                        if (jointSlot.transform == Builder.Root.transform && jointSlot.Slot.EquipedPart.Joint == PartJoint.JointType.BD)
                        {
                            PartBDTypeBind.transform.parent.gameObject.SetActive(true);
                            PartBDMaskBind.transform.parent.gameObject.SetActive(false);
                            for (int i = 0; i < 4; i++)
                            {
                                if (jointSlot.Slot.EquipedPart.BDMask.HasFlag(i))
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
                        if (jointSlot.Slot.EquipedPart.MainWeapon != null)
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
                        IsJBind.isOn = jointSlot.Slot.EquipedPart.IsJ;
                        SlotsBind.text = jointSlot.Slot.EquipedPart.Tunes.Count.ToString();
                    }
                    Builder.IgnoreEvents = false;
                }
                else PartEditSection.gameObject.SetActive(false);
            }
            else PartEditSection.gameObject.SetActive(false);
        }
        #endregion

        #region Utils
        public void UpdateMainDamage()
        {
            int.TryParse(MainForceBind.text, out int force);
            MainDMGBind.text = WeaponStats.CalculateDamage(force, Builder.AssembledData.TotalStats.TEC, true).ToString();
        }

        public void UpdateSubDamage()
        {
            int.TryParse(SubForceBind.text, out int force);
            SubDMGBind.text = WeaponStats.CalculateDamage(force, Builder.AssembledData.TotalStats.STR, false).ToString();
        }

        public void UpdateInfoType()
        {
            if (Builder.Root.Slot.EquipedPart != null)
            {
                string text = "";
                for(int i = 0; i < 4; i++)
                {
                    string typeName = "";
                    switch(i)
                    {
                        case 0:
                            typeName = "Lnd";
                            break;
                        case 1:
                            typeName = "Air";
                            break;
                        case 2:
                            typeName = "Art";
                            break;
                        case 3:
                            typeName = "Sup";
                            break;
                        default:
                            typeName = "Null";
                            break;
                    }

                    if (i > 0 && i < 4) text += " | ";

                    if (Builder.Root.Slot.EquipedPart.BDMask.HasFlag(i)) text += typeName;
                    else text += "---";
                }
                InfoTypeBind.text = text;
            }
            else InfoTypeBind.text = "--- | --- | --- | ---";
        }

        public void UpdateInfoHP()
        {
            InfoHPBind.text = Builder.AssembledData.TotalStats.HP.ToString();
        }

        public void UpdateInfoSize()
        {
            InfoSizeBind.text = "--";
            if (Builder.Root.Slot != null)
            {
                if (Builder.Root.Slot.EquipedPart != null) InfoSizeBind.text = Builder.Root.Slot.EquipedPart.Size == Part.SizeType.None ? "--" : Builder.Root.Slot.EquipedPart.Size.ToString();
            }
        }

        public void UpdateInfoCost()
        {
            InfoCostBind.text = Builder.AssembledData.TotalStats.COST.ToString();
        }

        public void UpdateInfoCapa()
        {
            InfoCapaBind.text = Builder.AssembledData.TotalStats.CAPA.ToString();
        }

        public void UpdateInfoStr()
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(Builder.AssembledData.TotalStats.STR, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            InfoStrBarBind.text = text;
            InfoStrBind.text = Builder.AssembledData.TotalStats.STR.ToString();
        }

        public void UpdateInfoTec()
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(Builder.AssembledData.TotalStats.TEC, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            InfoTecBarBind.text = text;
            InfoTecBind.text = Builder.AssembledData.TotalStats.TEC.ToString();
        }

        public void UpdateInfoWlk()
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(Builder.AssembledData.TotalStats.WLK, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            InfoWlkBarBind.text = text;
            InfoWlkBind.text = Builder.AssembledData.TotalStats.WLK.ToString();
        }

        public void UpdateInfoFly()
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(Builder.AssembledData.TotalStats.FLY, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            InfoFlyBarBind.text = text;
            InfoFlyBind.text = Builder.AssembledData.TotalStats.FLY.ToString();
        }

        public void UpdateInfoTgh()
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(Builder.AssembledData.TotalStats.TGH, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            InfoTghBarBind.text = text;
            InfoTghBind.text = Builder.AssembledData.TotalStats.TGH.ToString();
        }

        public void UpdateInfoLevel()
        {
            InfoLevelBind.text = Builder.AssembledData.MaxLevel.ToString();
        }
        #endregion
    }
}
