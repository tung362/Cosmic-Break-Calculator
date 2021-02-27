using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;
using CB.Calculator.Utils;
using CB.Calculator.UI;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic tune for parts
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class Tune
    {
        #region Format
        [MessagePackObject]
        public class CraftMaterial
        {
            [Key(0)]
            public string Name = "";
            [Key(1)]
            public int Amount = 0;
        }
        #endregion

        /*Enums*/
        public enum RequirementType { BD, LG, HD, HAC, FAC, AM, BS, WP, WB, Main, Sub }

        /*Configuration*/
        [Key(0)]
        public string Name = "";
        [Key(1)]
        public Color TuneColor = DefaultColors.RegularTune;
        [Key(2)]
        public Bitmask RequirementMask = new Bitmask();
        [Key(3)]
        public string Description = "";
        [Key(4)]
        public bool IsEx;
        [Key(5)]
        public int SuccessRate = 55;
        [Key(6)]
        public Stats Stats = new Stats();
        [Key(7)]
        public WeaponStats MainStats = new WeaponStats();
        [Key(8)]
        public WeaponStats.Multiplier MainMultiplier = new WeaponStats.Multiplier();
        [Key(9)]
        public WeaponStats SubStats = new WeaponStats();
        [Key(10)]
        public WeaponStats.Multiplier SubMultiplier = new WeaponStats.Multiplier();
        [Key(11)]
        public List<CraftMaterial> CraftMaterials = new List<CraftMaterial>();
    }
}
