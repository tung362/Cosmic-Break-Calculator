using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;
using CB.Calculator.Utils;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Handles creation and editing of PartJoints and Parts
    /// </summary>
    public class UIPartEditModule : MonoBehaviour
    {
        public static UIPartEditModule instance { get; private set; }

        /*Binds*/
        //Joint
        public TMP_Dropdown JointTypeBind;
        //Part
        public TMP_InputField PartNameBind;
        public TMP_InputField PartTagsBind;
        public UIMaskDropdown PartBDMaskBind;
        public TMP_Dropdown PartSizeBind;
        public TMP_InputField PartCOSTBind;
        public TMP_InputField PartCAPABind;
        public TMP_InputField PartHPBind;
        public TMP_InputField PartSTRBind;
        public TMP_InputField PartTECBind;
        public TMP_InputField PartWLKBind;
        public TMP_InputField PartFLYBind;
        public TMP_InputField PartTGHBind;
        public TMP_InputField PartMainForceBind;
        public TMP_InputField PartMainAmmoBind;
        public TMP_InputField PartMainRangeBind;
        public TMP_InputField PartMainSpeedBind;
        public TMP_InputField PartMainIntBind;
        public TMP_InputField PartSubForceBind;
        public TMP_InputField PartSubAmmoBind;
        public TMP_InputField PartSubRangeBind;
        public TMP_InputField PartSubSpeedBind;
        public TMP_InputField PartSubIntBind;
        public TMP_InputField PartDescriptionBind;
        public TMP_InputField PartTunesBind;

        /*Slots to edit*/
        public List<PartJointSlot> Slots;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"UIPartEditModule\"");
        }

        #region Utils
        public void UpdateJointEditable(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.Editable = toggle;
                Slots[i].TypeText.color = Slots[i].Slot.Editable ? Color.red : DefaultColors.Name;
            }
        }

        public void UpdateJointType()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.Joint = (PartJoint.JointType)JointTypeBind.value;
                Slots[i].TypeText.text = Slots[i].Slot.Joint.ToString();
                Slots[i].TypeIcon.sprite = Calculator.instance.JointIcons.Icons[Slots[i].Slot.Joint];
            }
        }

        public void UpdatePartName()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.EquipedPart.Name = PartNameBind.text;
                Slots[i].NameText.text = Slots[i].Slot.EquipedPart.Name;
            }
        }

        public void UpdatePartTags()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                //Grabs raw tags
                string[] tags = PartTagsBind.text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                //Filters out duplicate tags
                Dictionary<string, string> filteredTags = new Dictionary<string, string>();
                for(int j = 0; j < tags.Length; j++)
                {
                    if (!filteredTags.ContainsKey(tags[j])) filteredTags.Add(tags[j], tags[j]);
                }
                Slots[i].Slot.EquipedPart.Tags = filteredTags;
            }
        }

        public void UpdatePartBDMask()
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BDMask = PartBDMaskBind.Value;
        }

        public void UpdatePartSize()
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.Size = (Part.SizeType)PartSizeBind.value;
        }

        public void UpdatePartAllowJs(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.AllowJs = toggle;
        }

        public void UpdatePartIsJ(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.IsJ = toggle;
        }

        public void UpdatePartCOST()
        {
            int stat;
            if(int.TryParse(PartCOSTBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.COST = stat;
            }
        }

        public void UpdatePartCAPA()
        {
            int stat;
            if (int.TryParse(PartCAPABind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.CAPA = stat;
            }
        }

        public void UpdatePartHP()
        {
            int stat;
            if (int.TryParse(PartHPBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.HP = stat;
            }
        }

        public void UpdatePartSTR()
        {
            int stat;
            if (int.TryParse(PartSTRBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.STR = stat;
            }
        }

        public void UpdatePartTEC()
        {
            int stat;
            if (int.TryParse(PartTECBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.TEC = stat;
            }
        }

        public void UpdatePartWLK()
        {
            int stat;
            if (int.TryParse(PartWLKBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.WLK = stat;
            }
        }

        public void UpdatePartFLY()
        {
            int stat;
            if (int.TryParse(PartFLYBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.FLY = stat;
            }
        }

        public void UpdatePartTGH()
        {
            int stat;
            if (int.TryParse(PartTGHBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.TGH = stat;
            }
        }

        public void UpdatePartMainForce()
        {
            int stat;
            if (int.TryParse(PartMainForceBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Force = stat;
            }
        }

        public void UpdatePartMainAmmo()
        {
            int stat;
            if (int.TryParse(PartMainAmmoBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Ammo = stat;
            }
        }

        public void UpdatePartMainRange()
        {
            int stat;
            if (int.TryParse(PartMainRangeBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Range = stat;
            }
        }

        public void UpdatePartMainSpeed()
        {
            int stat;
            if (int.TryParse(PartMainSpeedBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Speed = stat;
            }
        }

        public void UpdatePartMainInt()
        {
            int stat;
            if (int.TryParse(PartMainIntBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Int = stat;
            }
        }

        public void UpdatePartSubForce()
        {
            int stat;
            if (int.TryParse(PartSubForceBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Force = stat;
            }
        }

        public void UpdatePartSubAmmo()
        {
            int stat;
            if (int.TryParse(PartSubAmmoBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Ammo = stat;
            }
        }

        public void UpdatePartSubRange()
        {
            int stat;
            if (int.TryParse(PartSubRangeBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Range = stat;
            }
        }

        public void UpdatePartSubSpeed()
        {
            int stat;
            if (int.TryParse(PartSubSpeedBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Speed = stat;
            }
        }

        public void UpdatePartSubInt()
        {
            int stat;
            if (int.TryParse(PartSubIntBind.text, out stat))
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Int = stat;
            }
        }

        public void UpdatePartDescription()
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.Description = PartDescriptionBind.text;
        }

        public void UpdatePartTunes()
        {
            int tuneCount;
            if (int.TryParse(PartTunesBind.text, out tuneCount))
            {
                for (int i = 0; i < Slots.Count; i++)
                {
                    for(int j = 0; j < tuneCount; j++)
                    {
                        Slots[i].Slot.EquipedPart.Tunes.Add(new Tune());
                        //Todo: Generate slot prefab
                    }
                }
            }
        }
        #endregion
    }
}
