using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Calculator.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic cartridge for the bot
    /// </summary>
    [System.Serializable]
    public class Cartridge
    {
        /*Configuration*/
        public int RequireLevel = 1;
        public Stats ModifierStats = new Stats();
        public WeaponStats ModifierWeaponStats = new WeaponStats();
        public float CoreForceMultiplier = 1.0f;
        public float CoreAmmoMultiplier = 1.0f;
        public float CoreRangeMultiplier = 1.0f;
        public float CoreSpeedMultiplier = 1.0f;
        public float CoreIntMultiplier = 1.0f;
        public string Description = "";
    }
}
