using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// UI trigger for fitting a region using defined points when enabled
    /// </summary>
    [ExecuteInEditMode]
    public class UIEnableSectionFitTrigger : MonoBehaviour
    {
        #region Format
        [System.Serializable]
        public class Section
        {
            public RectTransform UITransform;
            public RectTransform Left;
            public RectTransform Right;
            public RectTransform Bottom;
            public RectTransform Top;
        }
        #endregion

        public RectTransform ScreenCanvas;
        public List<Section> Sections = new List<Section>();

        void OnEnable()
        {
            Resize();
        }

        #region Utils
        public void Resize()
        {
            for(int i = 0; i < Sections.Count; i++)
            {
                if(Sections[i].UITransform)
                {
                    Vector2 min = new Vector2(Sections[i].Left ? Sections[i].Left.position.x : 0, Sections[i].Bottom ? Sections[i].Bottom.position.y : 0);
                    Vector2 max = new Vector2(Sections[i].Right ? Sections[i].Right.position.x - ScreenCanvas.rect.width : 0, Sections[i].Top ? Sections[i].Top.position.y - ScreenCanvas.rect.height : 0);
                    Sections[i].UITransform.offsetMin = min;
                    Sections[i].UITransform.offsetMax = max;
                }
            }
        }
        #endregion
    }
}
