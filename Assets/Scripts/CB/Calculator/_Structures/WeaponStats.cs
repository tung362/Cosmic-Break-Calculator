using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic weapon attachment for parts
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class WeaponStats
    {
        #region Format
        [MessagePackObject]
        public class Multiplier
        {
            [Key(0)]
            public float Force = 1.0f;
            [Key(1)]
            public float Ammo = 1.0f;
            [Key(2)]
            public float Range = 1.0f;
            [Key(3)]
            public float Speed = 1.0f;
            [Key(4)]
            public float Int = 1.0f;

            public override string ToString()
            {
                return $"Force: {Force}, Ammo: {Ammo}, Range: {Range}, Speed: {Speed}, Int: {Int}";
            }
        }
        #endregion

        /*Configuration*/
        [Key(0)]
        public int Force = 0;
        [Key(1)]
        public int Ammo = 0;
        [Key(2)]
        public int Range = 0;
        [Key(3)]
        public int Speed = 0;
        [Key(4)]
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

        public override string ToString()
        {
            return $"Force: {Force}, Ammo: {Ammo}, Range: {Range}, Speed: {Speed}, Int: {Int}";
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
