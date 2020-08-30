using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of Tune
    /// </summary>
    public class TuneSlot : MonoBehaviour
    {
        private Tune _Slot;
        public Tune Slot
        {
            get
            {
                return _Slot;
            }
            set
            {
                UpdateSlot();
            }
        }

        public Image Icon;
        public Color EmptySlotColor = new Color(0.4235294f, 0.5219815f, 0.5647059f);

        #region Utils
        public void UpdateSlot()
        {
            Icon.color = Slot != null ? Slot.TuneColor : EmptySlotColor;
        }
        #endregion
    }
}
