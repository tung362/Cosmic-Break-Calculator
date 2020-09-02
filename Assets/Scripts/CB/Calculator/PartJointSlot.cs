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

        public PartBuilder Builder;
        public TextMeshProUGUI TypeText;
        public Image TypeIcon;
        public RectTransform ChildLine;
        public TextMeshProUGUI NameText;
        public RectTransform TuneSlotOrigin;
        public Image SelectionBox;

        public void CreateJoint()
        {
            Builder.CreateJoint(this);
        }

        public void RemoveJoint()
        {
            Builder.RemoveJoint(this);
        }

        public void JointSelect()
        {
            Builder.JointSelect(this);
        }
    }
}
