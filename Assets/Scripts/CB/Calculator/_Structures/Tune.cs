using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;
using CB.Calculator.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic tune for parts
    /// </summary>
    //[System.Serializable]
    public class Tune : UIDescriptionModule.IUIDescription
    {
        /*Enums*/
        public enum RequirementType { BD, LG, HD, HAC, FAC, AM, BS, WP, WB, Main, Sub }

        /*Configuration*/
        public bool IsEx;
        public Bitmask RequirementMask = new Bitmask();
        public Color TuneColor = new Color(0.227451f, 0.8078431f, 0.2745098f);
        public Stats ModifierStats = new Stats();
        public WeaponStats ModifierWeaponStats = new WeaponStats();
        public float ForceMultiplier = 1.0f;
        public float AmmoMultiplier = 1.0f;
        public float RangeMultiplier = 1.0f;
        public float SpeedMultiplier = 1.0f;
        public float IntMultiplier = 1.0f;
        public string Description = "";

        public string GetDescription()
        {
            return Description;
        }
    }
}
