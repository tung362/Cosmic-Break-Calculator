using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching Image colors
    /// </summary>
    public class UIColorSwitch : MonoBehaviour
    {
        public Graphic Bind;
        public Color OnColor = Color.white;
        public Color OffColor = Color.white;

        public void SwitchColor(bool toggle)
        {
            Bind.color = toggle ? OnColor : OffColor;
        }
    }
}
