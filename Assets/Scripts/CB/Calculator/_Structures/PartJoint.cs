using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic joint slot
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class PartJoint
    {
        /*Enums*/
        public enum JointType { BD, LG, HD, HAC, FAC, AM, BS, WP, WB }

        /*Configuration*/
        [Key(0)]
        public bool Fixed = false;
        [Key(1)]
        public bool Required = false;
        [Key(2)]
        public bool AllowJs = true;
        [Key(3)]
        public Dictionary<string, string> Tags = new Dictionary<string, string>();
        [Key(4)]
        public JointType Joint = JointType.BD;

        /*Slot*/
        [Key(5)]
        public Part EquipedPart;
    }
}
