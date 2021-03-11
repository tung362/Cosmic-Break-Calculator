using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for resizing the LayoutElement
    /// </summary>
    public class UILayoutElementResize : MonoBehaviour
    {
        public LayoutElement Bind;

        public void Resize(Vector2 size)
        {
            Bind.preferredWidth = size.x;
            Bind.preferredHeight = size.y;
        }
    }
}
