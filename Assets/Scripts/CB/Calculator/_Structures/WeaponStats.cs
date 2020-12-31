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

        public WeaponStats()
        {
        }

        public WeaponStats(int force, int ammo, int range, int speed, int interval)
        {
            Force = force;
            Ammo = ammo;
            Range = range;
            Speed = speed;
            Int = interval;
        }

        public static WeaponStats operator +(WeaponStats s1, WeaponStats s2)
        {
            return new WeaponStats(s1.Force + s2.Force, s1.Ammo + s2.Ammo, s1.Range + s2.Range, s1.Speed + s2.Speed, s1.Int + s2.Int);
        }

        public static WeaponStats operator -(WeaponStats s1, WeaponStats s2)
        {
            return new WeaponStats(s1.Force - s2.Force, s1.Ammo - s2.Ammo, s1.Range - s2.Range, s1.Speed - s2.Speed, s1.Int - s2.Int);
        }

        #region Utils
        public static int CalculateDamage(int force, int stat, bool isMain)
        {
            float coefficient = (isMain ? 16 : 24) / 1000.0f;
            return (int)(force * (1 + coefficient * (Mathf.Clamp(stat, 0, 40) - 10)) + 0.5f);
        }
        #endregion
    }
}
