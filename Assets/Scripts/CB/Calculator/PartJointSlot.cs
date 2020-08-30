using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of PartJoint
    /// </summary>
    public class PartJointSlot : MonoBehaviour
    {
        public PartJoint Slot;
        //public PartJoint Slot
        //{
        //    get
        //    {
        //        return _Slot;
        //    }
        //    set
        //    {
        //        _Slot = value;

        //    }
        //}

        public TextMeshProUGUI TypeText;
        public Image TypeIcon;
        public RectTransform ChildLine;
        public TextMeshProUGUI NameText;
        public RectTransform TuneSlotOrigin;

        #region Utils
        //public void UpdatePartJoint()
        //{
        //    TypeText.text = _Slot.Joint.ToString();
        //    TypeIcon.sprite = Run.instance.JointIcons.Icons[_Slot.Joint];
        //}
        #endregion
    }
}
