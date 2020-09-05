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
        public PartJointSlot JointSlotPrefab;
        public TuneSlot TuneSlotPrefab;
        public Vector2 JointSlotOffset = new Vector2(0, -26.90006f);
        public Vector2 TuneSlotOffset = new Vector2(21.90002f, 0);

        /*Slot to edit*/
        public PartJointSlot EditSlot;

        /*Callbacks*/
        public event Action<PartJointSlot> OnRedraw;

        /*Cache*/
        //Set to true when you want toggle events to be ignored (For loading toggle settings without triggering edits)
        public bool IgnoreToggles = false;
        void Start()
        {
            //Create the root joint
            Root.Slot = new PartJoint();
            Root.AddSlotButton.interactable = false;
            Root.AddSlotButtonText.color = DefaultColors.Name;
            Root.RemoveSlotButton.interactable = false;
            Root.RemoveSlotButtonText.color = DefaultColors.Name;
        }

        #region Creation And Removal
        public void Load(PartJointSlot jointSlot, Part loadData)
        {
            jointSlot.Slot.Joint = loadData.Joint;
            jointSlot.Slot.EquipedPart = loadData;
            for(int i = 0; i < loadData.SubJoints.Count; i++)
            {
                PartJointSlot jointSlotChild = Instantiate(JointSlotPrefab, jointSlot.transform);
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
                PartJointSlot jointSlotChild = Instantiate(JointSlotPrefab, jointSlot.JointSlotOrigin);
                jointSlotChild.Builder = this;
                jointSlotChild.Parent = jointSlot;
                jointSlotChild.Slot = new PartJoint();
                jointSlotChild.AddSlotButton.interactable = false;
                jointSlotChild.AddSlotButtonText.color = DefaultColors.Name;
                jointSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(JointSlotOffset.x, JointSlotOffset.y * jointSlot.SubJoints.Count);
                jointSlot.Slot.EquipedPart.SubJoints.Add(jointSlotChild.Slot);
                jointSlot.SubJoints.Add(jointSlotChild);
            }
        }

        public void CreatePart(PartJointSlot jointSlot)
        {
            //Ensures that the part can only be created if the parent has a joint attached
            if(jointSlot.Slot != null)
            {
                jointSlot.Slot.EquipedPart = new Part();
                jointSlot.Slot.EquipedPart.Joint = jointSlot.Slot.Joint;
                if (jointSlot.transform == Root.transform && jointSlot.Slot.EquipedPart.Joint == PartJoint.JointType.BD) jointSlot.Slot.EquipedPart.BDMask.AddFlag((int)Part.BDType.Lnd);
                else jointSlot.Slot.EquipedPart.BDMask = Root.Slot.EquipedPart.BDMask;
                jointSlot.Slot.EquipedPart.Size = Root.Slot.EquipedPart.Size;
                jointSlot.AddSlotButton.interactable = true;
                jointSlot.AddSlotButtonText.color = DefaultColors.AddSlot;
            }
        }

        public void CreateTune(PartJointSlot jointSlot)
        {
            //Ensures that the tune can only be created if the parent has a part attached
            if (jointSlot.Slot.EquipedPart != null)
            {
                TuneSlot tuneSlot = Instantiate(TuneSlotPrefab, jointSlot.TuneSlotOrigin);
                tuneSlot.Slot = new Tune();
                tuneSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(TuneSlotOffset.x * jointSlot.Tunes.Count, TuneSlotOffset.y);
                jointSlot.Slot.EquipedPart.Tunes.Add(tuneSlot.Slot);
                jointSlot.Tunes.Add(tuneSlot);
            }
        }

        public void RemoveJoint(PartJointSlot jointSlot)
        {
            //Ensures that the joint can only be removed if a parent exists
            if (jointSlot.Parent)
            {
                jointSlot.Parent.Slot.EquipedPart.SubJoints.Remove(jointSlot.Slot);
                jointSlot.Parent.SubJoints.Remove(jointSlot);
                Destroy(jointSlot.gameObject);
            }
        }

        public void RemovePart(PartJointSlot jointSlot)
        {
            //Ensures that the part can only be removed if the parent has a joint attached
            if (jointSlot.Slot != null)
            {
                for(int i = 0; i < jointSlot.SubJoints.Count; i++)
                {
                    RemoveJoint(jointSlot.SubJoints[i]);
                    i--;
                }

                for (int i = 0; i < jointSlot.Tunes.Count; i++)
                {
                    RemoveTune(jointSlot);
                    i--;
                }

                jointSlot.Slot.EquipedPart = null;
                jointSlot.AddSlotButton.interactable = false;
                jointSlot.AddSlotButtonText.color = DefaultColors.Name;
            }
        }

        public void RemoveTune(PartJointSlot jointSlot)
        {
            //Ensures that the tune can only be removed if the parent has a part attached
            if (jointSlot.Slot.EquipedPart != null)
            {
                TuneSlot slot = jointSlot.Tunes[jointSlot.Tunes.Count - 1];
                jointSlot.Slot.EquipedPart.Tunes.Remove(slot.Slot);
                jointSlot.Tunes.Remove(slot);
                Destroy(slot.gameObject);
            }
        }

        public void JointSelect(PartJointSlot jointSlot)
        {
            if (EditSlot != null)
            {
                if(EditSlot.transform == jointSlot.transform) return;
                EditSlot.SelectionBox.gameObject.SetActive(false);
            }
            jointSlot.SelectionBox.gameObject.SetActive(true);
            EditSlot = jointSlot;
            //Event callback
            OnRedraw?.Invoke(jointSlot);
        }

        public void EditSlotsReset()
        {
            if (EditSlot != null) EditSlot.SelectionBox.gameObject.SetActive(false);
            EditSlot = null;
        }
        #endregion

        #region Editing
        public void UpdateJointFixed(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            EditSlot.Slot.Fixed = toggle;
            EditSlot.TypeText.color = EditSlot.Slot.Fixed ? Color.red : DefaultColors.Name;
        }

        public void UpdateJointRequired(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            EditSlot.Slot.Required = toggle;
        }

        public void UpdateJointTags(string text)
        {
            if (EditSlot == null) return;
            //Grabs raw tags
            string[] tags = text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            //Filters out duplicate tags
            Dictionary<string, string> filteredTags = new Dictionary<string, string>();
            for (int j = 0; j < tags.Length; j++)
            {
                if (!filteredTags.ContainsKey(tags[j])) filteredTags.Add(tags[j], tags[j]);
            }
            EditSlot.Slot.Tags = filteredTags;
        }

        public void UpdateJointType(int num)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.Joint = (PartJoint.JointType)num;
            if (EditSlot.Slot.EquipedPart != null)
            {
                EditSlot.Slot.EquipedPart.Joint = EditSlot.Slot.Joint;
                if (EditSlot.transform == Root.transform && EditSlot.Slot.EquipedPart.Joint == PartJoint.JointType.BD)
                {
                    if(EditSlot.Slot.EquipedPart.BDMask.Mask == 0) EditSlot.Slot.EquipedPart.BDMask.AddFlag((int)Part.BDType.Lnd);
                    else
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            if(EditSlot.Slot.EquipedPart.BDMask.HasFlag(i))
                            {
                                EditSlot.Slot.EquipedPart.BDMask.ClearFlags();
                                EditSlot.Slot.EquipedPart.BDMask.AddFlag(i);
                                return;
                            }
                        }
                    }
                }
            }
            EditSlot.TypeText.text = EditSlot.Slot.Joint.ToString();
            EditSlot.TypeIcon.sprite = Calculator.instance.JointIcons.Icons[EditSlot.Slot.Joint];
        }

        public void UpdateJointPart(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            if (toggle) EditSlot.Builder.CreatePart(EditSlot);
            else EditSlot.Builder.RemovePart(EditSlot);
            //Event callback
            OnRedraw?.Invoke(EditSlot);
        }

        public void UpdatePartName(string text)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.EquipedPart.Name = text;
            EditSlot.NameText.text = EditSlot.Slot.EquipedPart.Name;
        }

        public void UpdatePartTags(string text)
        {
            if (EditSlot == null) return;
            //Grabs raw tags
            string[] tags = text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            //Filters out duplicate tags
            Dictionary<string, string> filteredTags = new Dictionary<string, string>();
            for (int j = 0; j < tags.Length; j++)
            {
                if (!filteredTags.ContainsKey(tags[j])) filteredTags.Add(tags[j], tags[j]);
            }
            EditSlot.Slot.EquipedPart.Tags = filteredTags;
        }

        public void UpdatePartBDMask(Bitmask mask)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.EquipedPart.BDMask = mask;
        }

        public void UpdatePartBDMask(int flag)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.EquipedPart.BDMask.ClearFlags();
            EditSlot.Slot.EquipedPart.BDMask.AddFlag(flag);
        }

        public void UpdatePartSize(int num)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.EquipedPart.Size = (Part.SizeType)num;
        }

        public void UpdatePartAllowJs(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            EditSlot.Slot.EquipedPart.AllowJs = toggle;
        }

        public void UpdatePartIsJ(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            EditSlot.Slot.EquipedPart.IsJ = toggle;
        }

        public void UpdatePartCOST(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.COST = stat;
        }

        public void UpdatePartCAPA(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.CAPA = stat;
        }

        public void UpdatePartHP(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.HP = stat;
        }

        public void UpdatePartSTR(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.STR = stat;
        }

        public void UpdatePartTEC(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.TEC = stat;
        }

        public void UpdatePartWLK(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.WLK = stat;
        }

        public void UpdatePartFLY(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.FLY = stat;
        }

        public void UpdatePartTGH(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.BaseStats.TGH = stat;
        }

        public void UpdatePartMain(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            if (toggle) EditSlot.Slot.EquipedPart.MainWeapon = new WeaponStats();
            else EditSlot.Slot.EquipedPart.MainWeapon = null;

            Color textColor = DefaultColors.Value;
            if (EditSlot.Slot.EquipedPart.MainWeapon != null && EditSlot.Slot.EquipedPart.SubWeapon != null) textColor = DefaultColors.MAINSUB;
            else if(EditSlot.Slot.EquipedPart.MainWeapon != null) textColor = DefaultColors.MAIN;
            else if (EditSlot.Slot.EquipedPart.SubWeapon != null) textColor = DefaultColors.SUB;
            EditSlot.NameText.color = textColor;

            //Event callback
            OnRedraw?.Invoke(EditSlot);
        }

        public void UpdatePartSub(bool toggle)
        {
            if (EditSlot == null || IgnoreToggles) return;
            if (toggle) EditSlot.Slot.EquipedPart.SubWeapon = new WeaponStats();
            else EditSlot.Slot.EquipedPart.SubWeapon = null;

            Color textColor = DefaultColors.Value;
            if (EditSlot.Slot.EquipedPart.MainWeapon != null && EditSlot.Slot.EquipedPart.SubWeapon != null) textColor = DefaultColors.MAINSUB;
            else if (EditSlot.Slot.EquipedPart.MainWeapon != null) textColor = DefaultColors.MAIN;
            else if (EditSlot.Slot.EquipedPart.SubWeapon != null) textColor = DefaultColors.SUB;
            EditSlot.NameText.color = textColor;

            //Event callback
            OnRedraw?.Invoke(EditSlot);
        }

        public void UpdatePartMainForce(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.MainWeapon.Force = stat;
        }

        public void UpdatePartMainAmmo(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.MainWeapon.Ammo = stat;
        }

        public void UpdatePartMainRange(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.MainWeapon.Range = stat;
        }

        public void UpdatePartMainSpeed(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.MainWeapon.Speed = stat;
        }

        public void UpdatePartMainInt(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.MainWeapon.Int = stat;
        }

        public void UpdatePartSubForce(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.SubWeapon.Force = stat;
        }

        public void UpdatePartSubAmmo(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.SubWeapon.Ammo = stat;
        }

        public void UpdatePartSubRange(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.SubWeapon.Range = stat;
        }

        public void UpdatePartSubSpeed(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.SubWeapon.Speed = stat;
        }

        public void UpdatePartSubInt(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int stat);
            EditSlot.Slot.EquipedPart.SubWeapon.Int = stat;
        }

        public void UpdatePartDescription(string text)
        {
            if (EditSlot == null) return;
            EditSlot.Slot.EquipedPart.Description = text;
        }

        public void UpdatePartTunes(string text)
        {
            if (EditSlot == null) return;
            int.TryParse(text, out int tuneCount);
            int difference = tuneCount - EditSlot.Slot.EquipedPart.Tunes.Count;
            for(int i = 0; i < Mathf.Abs(difference); i++)
            {
                if(difference > 0) CreateTune(EditSlot);
                if (difference < 0) RemoveTune(EditSlot);
            }
        }
        #endregion
    }
}
