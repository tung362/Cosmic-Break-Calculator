using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;
using CB.Calculator.Utils;
using CB.Calculator.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic tune for parts
    /// </summary>
    //[System.Serializable]
    public class Tune
    {
        #region Format
        public class CraftMaterial
        {
            public string Name = "";
            public int Amount = 1;
        }
        #endregion

        /*Enums*/
        public enum RequirementType { BD, LG, HD, HAC, FAC, AM, BS, WP, WB, Main, Sub }

        /*Configuration*/
        public string Name = "";
        public Color TuneColor = DefaultColors.RegularTune;
        public Bitmask RequirementMask = new Bitmask();
        public string Description = "";
        public int SuccessRate = 55;
        public bool IsEx;
        public Stats Stats = new Stats();
        public WeaponStats MainStats = new WeaponStats();
        public WeaponStats.Multiplier MainMultiplier = new WeaponStats.Multiplier();
        public WeaponStats SubStats = new WeaponStats();
        public WeaponStats.Multiplier SubMultiplier = new WeaponStats.Multiplier();
        public List<CraftMaterial> Materials = new List<CraftMaterial>();
    }
}
