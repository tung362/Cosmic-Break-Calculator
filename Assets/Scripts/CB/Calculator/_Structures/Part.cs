using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;
using CB.Calculator.UI;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic part
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class Part
    {
        /*Enums*/
        public enum BDType { Lnd, Air, Art, Sup, Count };
        public enum SizeType { None, SS, S, M, L, LL, Count };

        /*Configuration*/
        [Key(0)]
        public string Name = "";
        [Key(1)]
        public Dictionary<string, string> Tags = new Dictionary<string, string>();
        [Key(2)]
        public Bitmask BDMask = new Bitmask();
        [Key(3)]
        public PartJoint.JointType Joint = PartJoint.JointType.BD;
        [Key(4)]
        public SizeType Size = SizeType.M;
        [Key(5)]
        public bool IsJ = false;
        [Key(6)]
        public Stats BaseStats = new Stats();
        [Key(7)]
        public WeaponStats MainWeapon = null;
        [Key(8)]
        public WeaponStats SubWeapon = null;
        [Key(9)]
        public string Description = "";
        [Key(10)]
        public List<Tune> Tunes = new List<Tune>();

        /*Joints branch*/
        [Key(11)]
        public List<PartJoint> SubJoints = new List<PartJoint>();
    }
}
