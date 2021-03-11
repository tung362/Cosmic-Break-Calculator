using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for force updating the layout
    /// </summary>
    public class UILayoutUpdate : MonoBehaviour
    {
        public RectTransform Bind;

        public void ForceLayoutUpdate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(Bind);
        }
    }
}
