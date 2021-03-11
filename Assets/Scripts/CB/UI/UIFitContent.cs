using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// UI effects for resizing to fit a content
    /// </summary>
    public class UIFitContent : MonoBehaviour
    {
        public RectTransform Bind;
        public RectTransform Content;
        public float ScrollBarOffset = 7.0f;

        public void RecalculateContent(RectTransform slot)
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Bind.transform, Content.transform);
            Bind.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x) + ScrollBarOffset, Mathf.Abs(combinedBounds.min.y) + ScrollBarOffset);
        }
    }
}
