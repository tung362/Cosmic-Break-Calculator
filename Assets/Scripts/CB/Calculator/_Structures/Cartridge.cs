using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Calculator.UI;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic cartridge for the bot
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class Cartridge
    {
        /*Configuration*/
        [Key(0)]
        public string Name = "";
        [Key(1)]
        public string Description = "";
        [Key(2)]
        public Stats Stats = new Stats();
        [Key(3)]
        public WeaponStats MainStats = new WeaponStats();
        [Key(4)]
        public WeaponStats.Multiplier MainMultiplier = new WeaponStats.Multiplier();
        [Key(5)]
        public WeaponStats SubStats = new WeaponStats();
        [Key(6)]
        public WeaponStats.Multiplier SubMultiplier = new WeaponStats.Multiplier();
    }
}
