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
        public bool Editable = true;
        public bool Required = false;
        public JointType Joint = JointType.BD;
        public Dictionary<string, string> Tags = new Dictionary<string, string>();

        private Part _EquipedPart;
        public Part EquipedPart
        {
            get
            {
                return _EquipedPart;
            }

            set
            {
                _EquipedPart = value;
                RecreateSubJoints(_EquipedPart);
            }
        }

        /*Joints tree*/
        public List<PartJoint> SubJoints = new List<PartJoint>();

        #region Utils
        public void RecreateSubJoints(Part part)
        {
            //Clear the existing tree
            SubJoints.Clear();

            //Creates a new tree
            for (int i = 0; i < part.SubJoints.Count; i++)
            {
                //Recursive branching
                if(part.SubJoints[i].EquipedPart != null) RecreateSubJoints(part.SubJoints[i].EquipedPart);
                SubJoints.Add(part.SubJoints[i]);
            }
        }
        #endregion
    }
}
