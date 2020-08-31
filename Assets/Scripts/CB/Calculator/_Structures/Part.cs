﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;
using CB.Calculator.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic part
    /// </summary>
    [System.Serializable]
    public class Part : UIDescriptionModule.IUIDescription
    {
        /*Enums*/
        public enum BDType { Lnd, Air, Art, Sup };
        public enum SizeType { SS, S, M, L, LL};

        /*Configuration*/
        public string Name = "";
        public Dictionary<string, string> Tags = new Dictionary<string, string>();
        public PartJoint.JointType Joint = PartJoint.JointType.BD;
        public Bitmask BDMask = new Bitmask();
        public SizeType Size = SizeType.M;
        public bool AllowJs = true;
        public bool IsJ = false;
        public Stats BaseStats = new Stats();
        public WeaponStats MainWeapon = null;
        public WeaponStats SubWeapon = null;
        public string Description = "";
        public List<Tune> Tunes = new List<Tune>();

        /*Joints branch*/
        public List<PartJoint> SubJoints = new List<PartJoint>();

        public string GetDescription()
        {
            return Description;
        }
    }
}
