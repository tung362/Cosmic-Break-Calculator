using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI loader for cartridges editor
    /// </summary>
    public class CartridgeEditLoader : MonoBehaviour
    {
        public CartridgeBuilder Builder;

        [Header("Cartridge")]
        public TMP_InputField NameBind;
        public TMP_InputField DiscriptionBind;
        [Header("Modifiers")]
        public TMP_InputField CostBind;
        public TMP_InputField HPBind;
        public TMP_InputField STRBind;
        public TMP_InputField TECBind;
        public TMP_InputField WLKBind;
        public TMP_InputField FLYBind;
        public TMP_InputField TGHBind;
        public TMP_InputField CAPABind;
        public TMP_InputField MainForceBind;
        public TMP_InputField MainAmmoBind;
        public TMP_InputField MainRangeBind;
        public TMP_InputField MainSpeedBind;
        public TMP_InputField MainIntBind;
        public TMP_InputField MainForceMultiplierBind;
        public TMP_InputField MainAmmoMultiplierBind;
        public TMP_InputField MainRangeMultiplierBind;
        public TMP_InputField MainSpeedMultiplierBind;
        public TMP_InputField MainIntMultiplierBind;
        public TMP_InputField SubForceBind;
        public TMP_InputField SubAmmoBind;
        public TMP_InputField SubRangeBind;
        public TMP_InputField SubSpeedBind;
        public TMP_InputField SubIntBind;
        public TMP_InputField SubForceMultiplierBind;
        public TMP_InputField SubAmmoMultiplierBind;
        public TMP_InputField SubRangeMultiplierBind;
        public TMP_InputField SubSpeedMultiplierBind;
        public TMP_InputField SubIntMultiplierBind;

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

        void OnRedraw(Cartridge cartridge)
        {
            Builder.IgnoreEvents = true;
            /*Cartridge*/
            NameBind.text = Builder.AssembledData.Name;
            DiscriptionBind.text = Builder.AssembledData.Description;
            /*Modifiers*/
            CostBind.text = Builder.AssembledData.Stats.COST.ToString();
            HPBind.text = Builder.AssembledData.Stats.HP.ToString();
            STRBind.text = Builder.AssembledData.Stats.STR.ToString();
            TECBind.text = Builder.AssembledData.Stats.TEC.ToString();
            WLKBind.text = Builder.AssembledData.Stats.WLK.ToString();
            FLYBind.text = Builder.AssembledData.Stats.FLY.ToString();
            TGHBind.text = Builder.AssembledData.Stats.TGH.ToString();
            CAPABind.text = Builder.AssembledData.Stats.CAPA.ToString();
            MainForceBind.text = Builder.AssembledData.MainStats.Force.ToString();
            MainAmmoBind.text = Builder.AssembledData.MainStats.Ammo.ToString();
            MainRangeBind.text = Builder.AssembledData.MainStats.Range.ToString();
            MainSpeedBind.text = Builder.AssembledData.MainStats.Speed.ToString();
            MainIntBind.text = Builder.AssembledData.MainStats.Int.ToString();
            MainForceMultiplierBind.text = Builder.AssembledData.MainMultiplier.Force.ToString();
            MainAmmoMultiplierBind.text = Builder.AssembledData.MainMultiplier.Ammo.ToString();
            MainRangeMultiplierBind.text = Builder.AssembledData.MainMultiplier.Range.ToString();
            MainSpeedMultiplierBind.text = Builder.AssembledData.MainMultiplier.Speed.ToString();
            MainIntMultiplierBind.text = Builder.AssembledData.MainMultiplier.Int.ToString();
            SubForceBind.text = Builder.AssembledData.SubStats.Force.ToString();
            SubAmmoBind.text = Builder.AssembledData.SubStats.Ammo.ToString();
            SubRangeBind.text = Builder.AssembledData.SubStats.Range.ToString();
            SubSpeedBind.text = Builder.AssembledData.SubStats.Speed.ToString();
            SubIntBind.text = Builder.AssembledData.SubStats.Int.ToString();
            SubForceMultiplierBind.text = Builder.AssembledData.SubMultiplier.Force.ToString();
            SubAmmoMultiplierBind.text = Builder.AssembledData.SubMultiplier.Ammo.ToString();
            SubRangeMultiplierBind.text = Builder.AssembledData.SubMultiplier.Range.ToString();
            SubSpeedMultiplierBind.text = Builder.AssembledData.SubMultiplier.Speed.ToString();
            SubIntMultiplierBind.text = Builder.AssembledData.SubMultiplier.Int.ToString();
            Builder.IgnoreEvents = false;
        }
    }
}
