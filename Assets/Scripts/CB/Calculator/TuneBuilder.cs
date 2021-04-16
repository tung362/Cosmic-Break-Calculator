using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CB.Utils;
using CB.Calculator.Utils;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of Tunes
    /// </summary>
    public class TuneBuilder : MonoBehaviour, IBuilder<Tune>
    {
        public Tune AssembledData = new Tune();
        public RectTransform Content;
        public RectTransform MaterialTemplate;
        public CraftMaterialSlot MaterialSlotTemplate;
        public RectTransform MaterialAddButton;
        public Vector2 MaterialOffset = new Vector2(0, -27);
        public float ScrollBarOffset = 7.0f;

        /*Callbacks*/
        public event Action<Tune> OnRedraw;

        /*Cache*/
        //Set to true when you want events to be ignored (For loading settings without triggering edits)
        public bool IgnoreEvents = false;
        private List<CraftMaterialSlot> MaterialSlots = new List<CraftMaterialSlot>();

        public void Init()
        {

        }

        void Start()
        {
            OnRedraw?.Invoke(AssembledData);
        }

        #region Creation And Removal
        public void CreateNew()
        {
            AssembledData = new Tune();

            //Materials
            for (int i = 0; i < MaterialSlots.Count; i++) Destroy(MaterialSlots[i].gameObject);
            MaterialSlots.Clear();
            MaterialAddButton.anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
            RecalculateContent();

            //Event callback
            OnRedraw?.Invoke(AssembledData);
        }

        public void Load(Tune loadData)
        {
            AssembledData = loadData;

            //Materials
            for(int i = 0; i < MaterialSlots.Count; i++) Destroy(MaterialSlots[i].gameObject);
            MaterialSlots.Clear();

            for (int i = 0; i < AssembledData.CraftMaterials.Count; i++)
            {
                CraftMaterialSlot craftMaterialSlot = Instantiate(MaterialSlotTemplate, MaterialTemplate.transform);
                craftMaterialSlot.gameObject.SetActive(true);
                craftMaterialSlot.NameInputField.SetTextWithoutNotify(AssembledData.CraftMaterials[i].Name);
                craftMaterialSlot.AmountInputField.SetTextWithoutNotify(AssembledData.CraftMaterials[i].Amount.ToString());
                craftMaterialSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
                MaterialSlots.Add(craftMaterialSlot);
            }
            MaterialAddButton.anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
            RecalculateContent();

            //Event callback
            OnRedraw?.Invoke(AssembledData);
        }

        public void RecalculateContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Content.transform, MaterialAddButton);
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, Mathf.Abs(combinedBounds.min.y) + ScrollBarOffset);
        }
        #endregion

        #region Editing
        public void UpdateName(string text)
        {
            if (IgnoreEvents) return;
            AssembledData.Name = text;
        }

        public void UpdateColor(Color color)
        {
            if (IgnoreEvents) return;
            AssembledData.TuneColor = color;
        }

        public void UpdateRequirementMask(Bitmask mask)
        {
            if (IgnoreEvents) return;
            AssembledData.RequirementMask = mask;
        }

        public void UpdateRequirementMask(int flag)
        {
            if (IgnoreEvents) return;
            AssembledData.RequirementMask.ClearFlags();
            AssembledData.RequirementMask.AddFlag(flag);
        }

        public void UpdateDescription(string text)
        {
            if (IgnoreEvents) return;
            AssembledData.Description = text;
        }

        public void UpdateIsEx(bool toggle)
        {
            if (IgnoreEvents) return;
            AssembledData.IsEx = toggle;
        }

        public void UpdateSuccessRate(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int rate);
            AssembledData.SuccessRate = rate;
        }

        public void UpdateCOST(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.COST = stat;
        }

        public void UpdateCAPA(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.CAPA = stat;
        }

        public void UpdateHP(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.HP = stat;
        }

        public void UpdateSTR(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.STR = stat;
        }

        public void UpdateTEC(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.TEC = stat;
        }

        public void UpdateWLK(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.WLK = stat;
        }

        public void UpdateFLY(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.FLY = stat;
        }

        public void UpdateTGH(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.Stats.TGH = stat;
        }

        public void UpdateMainForce(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.MainStats.Force = stat;
        }

        public void UpdateMainAmmo(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.MainStats.Ammo = stat;
        }

        public void UpdateMainRange(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.MainStats.Range = stat;
        }

        public void UpdateMainSpeed(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.MainStats.Speed = stat;
        }

        public void UpdateMainInt(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.MainStats.Int = stat;
        }

        public void UpdateMainMultiplierForce(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.MainMultiplier.Force = stat;
        }

        public void UpdateMainMultiplierAmmo(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.MainMultiplier.Ammo = stat;
        }

        public void UpdateMainMultiplierRange(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.MainMultiplier.Range = stat;
        }

        public void UpdateMainMultiplierSpeed(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.MainMultiplier.Speed = stat;
        }

        public void UpdateMainMultiplierInt(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.MainMultiplier.Int = stat;
        }

        public void UpdateSubForce(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.SubStats.Force = stat;
        }

        public void UpdateSubAmmo(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.SubStats.Ammo = stat;
        }

        public void UpdateSubRange(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.SubStats.Range = stat;
        }

        public void UpdateSubSpeed(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.SubStats.Speed = stat;
        }

        public void UpdateSubInt(string text)
        {
            if (IgnoreEvents) return;
            int.TryParse(text, out int stat);
            AssembledData.SubStats.Int = stat;
        }

        public void UpdateSubMultiplierForce(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.SubMultiplier.Force = stat;
        }

        public void UpdateSubMultiplierAmmo(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.SubMultiplier.Ammo = stat;
        }

        public void UpdateSubMultiplierRange(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.SubMultiplier.Range = stat;
        }

        public void UpdateSubMultiplierSpeed(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.SubMultiplier.Speed = stat;
        }

        public void UpdateSubMultiplierInt(string text)
        {
            if (IgnoreEvents) return;
            float.TryParse(text, out float stat);
            AssembledData.SubMultiplier.Int = stat;
        }

        public void UpdateMaterialName(CraftMaterialSlot craftMaterialSlot, string text)
        {
            if (IgnoreEvents) return;
            int index = MaterialSlots.IndexOf(craftMaterialSlot);
            AssembledData.CraftMaterials[index].Name = text;
        }

        public void UpdateMaterialAmount(CraftMaterialSlot craftMaterialSlot, string text)
        {
            if (IgnoreEvents) return;
            int index = MaterialSlots.IndexOf(craftMaterialSlot);
            int.TryParse(text, out int amount);
            AssembledData.CraftMaterials[index].Amount = amount;
        }

        public void AddMaterialSlot()
        {
            CraftMaterialSlot craftMaterialSlot = Instantiate(MaterialSlotTemplate, MaterialTemplate.transform);
            craftMaterialSlot.gameObject.SetActive(true);
            craftMaterialSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
            MaterialSlots.Add(craftMaterialSlot);
            AssembledData.CraftMaterials.Add(new Tune.CraftMaterial());

            MaterialAddButton.anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
            RecalculateContent();
        }

        public void RemoveMaterialSlot(CraftMaterialSlot craftMaterialSlot)
        {
            int index = MaterialSlots.IndexOf(craftMaterialSlot);
            MaterialSlots.RemoveAt(index);
            AssembledData.CraftMaterials.RemoveAt(index);
            Destroy(craftMaterialSlot.gameObject);
            for (int i = index; i < MaterialSlots.Count; i++)
            {
                RectTransform slotTransform = MaterialSlots[i].GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(slotTransform.anchoredPosition.x, slotTransform.anchoredPosition.y - MaterialOffset.y);
            }

            MaterialAddButton.anchoredPosition = new Vector2(MaterialOffset.x, MaterialOffset.y * MaterialSlots.Count);
            RecalculateContent();
        }
        #endregion
    }
}
