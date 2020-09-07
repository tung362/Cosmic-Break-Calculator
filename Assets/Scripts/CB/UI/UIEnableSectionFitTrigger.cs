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
                    float xMin = Sections[i].Left && Sections[i].Left.gameObject.activeInHierarchy ? Sections[i].Left.position.x : 0;
                    float yMin = Sections[i].Bottom && Sections[i].Bottom.gameObject.activeInHierarchy ? Sections[i].Bottom.position.y : 0;
                    float xMax = Sections[i].Right && Sections[i].Right.gameObject.activeInHierarchy ? Sections[i].Right.position.x - ScreenCanvas.rect.width : 0;
                    float yMax = Sections[i].Top && Sections[i].Top.gameObject.activeInHierarchy ? Sections[i].Top.position.y - ScreenCanvas.rect.height : 0;
                    Sections[i].UITransform.offsetMin = new Vector2(xMin, yMin);
                    Sections[i].UITransform.offsetMax = new Vector2(xMax, yMax);
                }
            }
        }
        #endregion
    }
}
