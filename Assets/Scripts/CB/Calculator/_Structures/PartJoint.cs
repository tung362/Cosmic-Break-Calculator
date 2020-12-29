using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Calculator
{
    /// <summary>
    /// Represents a generic joint slot
    /// </summary>
    //[System.Serializable]
    public class PartJoint
    {
        /*Enums*/
        public enum JointType { BD, LG, HD, HAC, FAC, AM, BS, WP, WB }

        /*Configuration*/
        public bool Fixed = false;
        public bool Required = false;
        public bool AllowJs = true;
        public Dictionary<string, string> Tags = new Dictionary<string, string>();
        public JointType Joint = JointType.BD;

        public Part EquipedPart;
    }
}
