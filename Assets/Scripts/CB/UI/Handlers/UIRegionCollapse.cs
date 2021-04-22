using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// UI effect for collapsing a defined region when window is resized
    /// To prevent race conditions please ensure Regions is ordered by first to collapse
    /// </summary>
    public class UIRegionCollapse : MonoBehaviour
    {
        #region Format
        [System.Serializable]
        public class Region
        {
            public RectTransform UITransform;
            public RectTransform ThresholdX;
            public RectTransform ThresholdY;
            public bool ReverseX = false;
            public bool ReverseY = false;

            [HideInInspector]
            public Vector2 OriginalSize;
        }
        #endregion

        public List<Region> Regions = new List<Region>();

        void OnEnable()
        {
            //Set listener
            WindowResizeEvent.OnWindowResize += OnWindowResize;
            for (int i = 0; i < Regions.Count; i++) Regions[i].OriginalSize = Regions[i].UITransform.sizeDelta;
            OnWindowResize();
        }

        void OnDisable()
        {
            //Unset listener
            WindowResizeEvent.OnWindowResize -= OnWindowResize;
            for (int i = 0; i < Regions.Count; i++) Regions[i].UITransform.sizeDelta = Regions[i].OriginalSize;
        }

        #region Listeners
        void OnWindowResize()
        {
            //Collapse regions based on axis
            for (int i = 0; i < Regions.Count; i++)
            {
                Vector2 resize = Regions[i].OriginalSize;
                if (Regions[i].ThresholdX)
                {
                    Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Regions[i].UITransform, Regions[i].ThresholdX);
                    float size = Regions[i].ReverseX ? -bounds.min.x : bounds.max.x;
                    if (size < Regions[i].OriginalSize.x) resize.x = size;
                }

                if (Regions[i].ThresholdY)
                {
                    Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Regions[i].UITransform, Regions[i].ThresholdY);
                    float size = Regions[i].ReverseY ? -bounds.min.y : bounds.max.y;
                    if (size < Regions[i].OriginalSize.y) resize.y = size;
                }
                Regions[i].UITransform.sizeDelta = resize;
            }
        }
        #endregion
        public void RemoveAllEmpty()
        {
            //Remove all unused
            for (int i = 0; i < Regions.Count; i++)
            {
                if(!Regions[i].UITransform)
                {
                    Regions.RemoveAt(i);
                    i--;
                }
            }
        }
        #region Utils
        #endregion
    }
}
