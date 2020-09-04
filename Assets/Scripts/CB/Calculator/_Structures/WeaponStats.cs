using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic weapon attachment for parts
    /// </summary>
    //[System.Serializable]
    public class WeaponStats
    {
        /*Configuration*/
        public int Force = 0;
        public int Ammo = 0;
        public int Range = 0;
        public int Speed = 0;
        public int Int = 0;

        #region Utils
        public static int CalculateDamage(int force, int stat, bool isMain)
        {
            float coefficient = (isMain ? 16 : 24) / 1000.0f;
            return (int)(force * (1 + coefficient * (Mathf.Clamp(stat, 0, 40) - 10)) + 0.5f);
        }
        #endregion
    }
}
