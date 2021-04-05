using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching Selectable colors
    /// </summary>
    public class UISelectableSwitch : MonoBehaviour
    {
        public Selectable Bind;
        [Header("On")]
        public ColorBlock OnBlock;
        [Header("Off")]
        public ColorBlock OffBlock;

        public void SwitchColors(bool toggle)
        {
            Bind.colors = toggle ? OnBlock : OffBlock;
        }
    }
}
