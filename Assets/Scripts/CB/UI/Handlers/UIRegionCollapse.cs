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
            public RectTransform Threshold;
            public bool Reverse = false;

            [HideInInspector]
            public Vector2 OriginalSize;
        }
        #endregion

        public List<Region> Regions = new List<Region>();
        public bool Horizontal = false;

        void OnEnable()
        {
            //Set listener
            WindowResizeEvent.OnWindowResize += OnWindowResize;
            for(int i = 0; i < Regions.Count; i++) Regions[i].OriginalSize = Regions[i].UITransform.sizeDelta;
            OnWindowResize();
        }

        void OnDisable()
        {
            //Unset listener
            WindowResizeEvent.OnWindowResize -= OnWindowResize;
            for(int i = 0; i < Regions.Count; i++) Regions[i].UITransform.sizeDelta = Regions[i].OriginalSize;
        }

        #region Listeners
        void OnWindowResize()
        {
            //Collapse regions based on axis
            for (int i = 0; i < Regions.Count; i++)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Regions[i].UITransform, Regions[i].Threshold.position, null, out Vector2 difference);

                Vector2 resize = Regions[i].OriginalSize;
                if (Horizontal)
                {
                    float size = Regions[i].Reverse ? -difference.x : difference.x;
                    if (size < Regions[i].OriginalSize.x) resize.x = size;
                }
                else
                {
                    float size = Regions[i].Reverse ? -difference.y : difference.y;
                    if (size < Regions[i].OriginalSize.y) resize.y = size;
                }
                Regions[i].UITransform.sizeDelta = resize;
            }
        }
        #endregion
    }
}
