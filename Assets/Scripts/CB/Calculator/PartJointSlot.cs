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
    public class PartJointSlot : MonoBehaviour, ISelectable
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
        public List<PartJointSlot> SubJoints = new List<PartJointSlot>();
        public List<TuneSlot> Tunes = new List<TuneSlot>();
        public int BranchCount = 1;
        public int BranchIndex;

        void OnDestroy()
        {
            //Invoke redraw event if the joint being destroyed is currently selected
            if(Builder.EditSlot)
            {
                if (Builder.EditSlot.transform == transform) Builder.EditSlotsReset();
            }
        }

        #region Utils
        public void HoverSelect()
        {
            Builder.SelectableHover(this);
        }

        public void Move()
        {

        }

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
