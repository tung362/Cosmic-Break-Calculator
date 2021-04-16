using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CB.Utils;
using CB.Calculator.Utils;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of Cartridges
    /// </summary>
    public class CartridgeBuilder : MonoBehaviour, IBuilder<Cartridge>
    {
        public Cartridge AssembledData = new Cartridge();

        /*Callbacks*/
        public event Action<Cartridge> OnRedraw;

        /*Cache*/
        //Set to true when you want events to be ignored (For loading settings without triggering edits)
        public bool IgnoreEvents = false;

        public void Init()
        {

        }

        void Start()
        {
            //Event callback
            OnRedraw?.Invoke(AssembledData);
        }

        #region Creation And Removal
        public void CreateNew()
        {
            AssembledData = new Cartridge();
            //Event callback
            OnRedraw?.Invoke(AssembledData);
        }

        public void Load(Cartridge loadData)
        {
            AssembledData = loadData;
            //Event callback
            OnRedraw?.Invoke(AssembledData);
        }
        #endregion

        #region Editing
        public void UpdateName(string text)
        {
            if (IgnoreEvents) return;
            AssembledData.Name = text;
        }

        public void UpdateDescription(string text)
        {
            if (IgnoreEvents) return;
            AssembledData.Description = text;
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
        #endregion
    }
}
