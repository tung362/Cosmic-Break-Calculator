using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// UI effects for resizing the UI
    /// </summary>
    public class UIResize : MonoBehaviour
    {
        public RectTransform Bind;
        public bool ResizeWidth = false;
        public bool ResizeHeight = false;

        public void Resize(Vector2 size)
        {
            Bind.sizeDelta = new Vector2(ResizeWidth ? size.x : Bind.sizeDelta.x, ResizeHeight ? size.y : Bind.sizeDelta.y);
        }
    }
}
