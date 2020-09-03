using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CB.Utils;
using CB.Calculator.Utils;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of Parts
    /// </summary>
    public class PartBuilder : MonoBehaviour
    {
        public PartJointSlot Root;
        public PartJointSlot SlotPrefab;

        /*Slots to edit*/
        public List<PartJointSlot> Slots;

        /*Callbacks*/
        public event Action<PartJointSlot> OnJointSelect;

        void Start()
        {
            //Create the root joint
            Root.Slot = new PartJoint();
        }

        #region Creation And Removal
        public void Load(PartJointSlot jointSlot, Part loadData)
        {
            jointSlot.Slot.Joint = loadData.Joint;
            jointSlot.Slot.EquipedPart = loadData;
            for(int i = 0; i < loadData.SubJoints.Count; i++)
            {
                PartJointSlot jointSlotChild = Instantiate(SlotPrefab, jointSlot.transform);
                jointSlotChild.Slot = loadData.SubJoints[i];

                //Recursive branching
                Load(jointSlotChild, jointSlotChild.Slot.EquipedPart);
            }
        }

        public void CreateJoint(PartJointSlot jointSlot)
        {
            //Ensures that the joint can only be created if the parent has a part attached
            if (jointSlot.Slot.EquipedPart != null)
            {
                PartJointSlot jointSlotChild = Instantiate(SlotPrefab, jointSlot.transform);
                jointSlotChild.Slot = new PartJoint();
                jointSlot.Slot.EquipedPart.SubJoints.Add(jointSlotChild.Slot);
            }
        }

        public void CreatePart(PartJointSlot jointSlot)
        {
            //Ensures that the part can only be created if the parent has a joint attached
            if(jointSlot.Slot != null)
            {
                jointSlot.Slot.EquipedPart = new Part();
                jointSlot.Slot.EquipedPart.Joint = jointSlot.Slot.Joint;
                jointSlot.Slot.EquipedPart.BDMask = Root.Slot.EquipedPart.BDMask;
                jointSlot.Slot.EquipedPart.Size = Root.Slot.EquipedPart.Size;
            }
        }

        public void RemoveJoint(PartJointSlot jointSlot)
        {

        }

        public void RemovePart(PartJointSlot jointSlot)
        {

        }

        public void JointSelect(PartJointSlot jointSlot)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                for (int i = 0; i < Slots.Count; i++)
                {
                    if (Slots[i].transform == jointSlot.transform) return;
                }
            }
            else
            {
                for (int i = 0; i < Slots.Count; i++) Slots[i].SelectionBox.gameObject.SetActive(false);
                Slots.Clear();
            }
            jointSlot.SelectionBox.gameObject.SetActive(true);
            Slots.Add(jointSlot);
            //Event callback
            OnJointSelect?.Invoke(jointSlot);
        }

        public void JointDeselect()
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].SelectionBox.gameObject.SetActive(false);
            Slots.Clear();
        }
        #endregion

        #region Editing
        public void UpdateJointEditable(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.Editable = toggle;
                Slots[i].TypeText.color = Slots[i].Slot.Editable ? Color.red : DefaultColors.Name;
            }
        }

        public void UpdateJointRequired(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.Required = toggle;
        }

        public void UpdateJointTags(string text)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                //Grabs raw tags
                string[] tags = text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                //Filters out duplicate tags
                Dictionary<string, string> filteredTags = new Dictionary<string, string>();
                for (int j = 0; j < tags.Length; j++)
                {
                    if (!filteredTags.ContainsKey(tags[j])) filteredTags.Add(tags[j], tags[j]);
                }
                Slots[i].Slot.Tags = filteredTags;
            }
        }

        public void UpdateJointType(int num)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.Joint = (PartJoint.JointType)num;
                Slots[i].TypeText.text = Slots[i].Slot.Joint.ToString();
                Slots[i].TypeIcon.sprite = Calculator.instance.JointIcons.Icons[Slots[i].Slot.Joint];
            }
        }

        public void UpdateJointPart(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if(toggle) Slots[i].Builder.CreatePart(Slots[i]);
                else Slots[i].Builder.RemovePart(Slots[i]);
            }
        }

        public void UpdatePartName(string text)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Slot.EquipedPart.Name = text;
                Slots[i].NameText.text = Slots[i].Slot.EquipedPart.Name;
            }
        }

        public void UpdatePartTags(string text)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                //Grabs raw tags
                string[] tags = text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                //Filters out duplicate tags
                Dictionary<string, string> filteredTags = new Dictionary<string, string>();
                for (int j = 0; j < tags.Length; j++)
                {
                    if (!filteredTags.ContainsKey(tags[j])) filteredTags.Add(tags[j], tags[j]);
                }
                Slots[i].Slot.EquipedPart.Tags = filteredTags;
            }
        }

        public void UpdatePartBDMask(Bitmask mask)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BDMask = mask;
        }

        public void UpdatePartSize(int num)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.Size = (Part.SizeType)num;
        }

        public void UpdatePartAllowJs(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.AllowJs = toggle;
        }

        public void UpdatePartIsJ(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.IsJ = toggle;
        }

        public void UpdatePartCOST(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.COST = stat;
        }

        public void UpdatePartCAPA(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.CAPA = stat;
        }

        public void UpdatePartHP(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.HP = stat;
        }

        public void UpdatePartSTR(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.STR = stat;
        }

        public void UpdatePartTEC(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.TEC = stat;
        }

        public void UpdatePartWLK(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.WLK = stat;
        }

        public void UpdatePartFLY(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.FLY = stat;
        }

        public void UpdatePartTGH(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.BaseStats.TGH = stat;
        }

        public void UpdatePartMain(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if(toggle) Slots[i].Slot.EquipedPart.MainWeapon = new WeaponStats();
                else Slots[i].Slot.EquipedPart.MainWeapon = null;
                Slots[i].NameText.color = Slots[i].Slot.EquipedPart.MainWeapon == null ? DefaultColors.Value : Slots[i].Slot.EquipedPart.SubWeapon == null ? DefaultColors.MAIN : DefaultColors.MAINSUB;
            }
        }

        public void UpdatePartSub(bool toggle)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (toggle) Slots[i].Slot.EquipedPart.SubWeapon = new WeaponStats();
                else Slots[i].Slot.EquipedPart.SubWeapon = null;
                Slots[i].NameText.color = Slots[i].Slot.EquipedPart.SubWeapon == null ? DefaultColors.Value : Slots[i].Slot.EquipedPart.MainWeapon == null ? DefaultColors.SUB : DefaultColors.MAINSUB;
            }
        }

        public void UpdatePartMainForce(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Force = stat;
        }

        public void UpdatePartMainAmmo(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Ammo = stat;
        }

        public void UpdatePartMainRange(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Range = stat;
        }

        public void UpdatePartMainSpeed(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Speed = stat;
        }

        public void UpdatePartMainInt(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Int = stat;
        }

        public void UpdatePartMainDMG(string text)
        {
            //int.TryParse(text, out int stat);
            //for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Int = stat;
        }

        public void UpdatePartSubForce(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Force = stat;
        }

        public void UpdatePartSubAmmo(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Ammo = stat;
        }

        public void UpdatePartSubRange(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Range = stat;
        }

        public void UpdatePartSubSpeed(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Speed = stat;
        }

        public void UpdatePartSubInt(string text)
        {
            int.TryParse(text, out int stat);
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.SubWeapon.Int = stat;
        }

        public void UpdatePartSubDMG(string text)
        {
            //int.TryParse(text, out int stat);
            //for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.MainWeapon.Int = stat;
        }

        public void UpdatePartDescription(string text)
        {
            for (int i = 0; i < Slots.Count; i++) Slots[i].Slot.EquipedPart.Description = text;
        }

        public void UpdatePartTunes(string text)
        {
            int.TryParse(text, out int tuneCount);

            for (int i = 0; i < Slots.Count; i++)
            {
                int difference = tuneCount - Slots[i].Slot.EquipedPart.Tunes.Count;
                for (int j = 0; j < Mathf.Abs(difference); j++)
                {
                    if(difference >= 0) Slots[i].Slot.EquipedPart.Tunes.Add(new Tune());
                    else Slots[i].Slot.EquipedPart.Tunes.RemoveAt(Slots[i].Slot.EquipedPart.Tunes.Count - 1);
                    //Todo: Generate slot prefab
                }
            }
        }
        #endregion
    }
}
