using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of Tune
    /// </summary>
    public class TuneSlot : MonoBehaviour, ISelectable
    {
        //Hook to an existing Tune
        public Tune Slot;

        [Header("Binds")]
        public Image Icon;

        [Header("Builder")]
        public PartBuilder Builder;

        #region Utils
        public void HoverSelect()
        {
            Builder.SelectableHover(this);
        }
        #endregion
    }
}
