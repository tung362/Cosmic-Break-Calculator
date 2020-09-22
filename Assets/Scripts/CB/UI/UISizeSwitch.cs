using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching Image colors
    /// </summary>
    public class UISizeSwitch : MonoBehaviour
    {
        public RectTransform Bind;
        public Vector2 OnSize;
        public Vector2 OffSize;

        public void SwitchSize(bool toggle)
        {
            Bind.sizeDelta = toggle ? OnSize : OffSize;
        }
    }
}
