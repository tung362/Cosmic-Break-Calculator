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
        //Hook to an existing PartJoint
        public PartJoint Slot;

        [Header("Binds")]
        public TextMeshProUGUI TypeText;
        public Image TypeIcon;
        public RectTransform ChildLine;
        public TextMeshProUGUI NameText;
        public Image SelectionBox;
        public Button AddSlotButton;
        public TMP_Text AddSlotButtonText;
        public Button RemoveSlotButton;
        public TMP_Text RemoveSlotButtonText;
        public RectTransform JointSlotOrigin;
        public RectTransform TuneSlotOrigin;

        [Header("Builder")]
        public PartBuilder Builder;
        public PartJointSlot Parent;
        public List<PartJointSlot> SubJoints;
        public List<TuneSlot> Tunes;

        #region Utils
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
        #endregion
    }
}
