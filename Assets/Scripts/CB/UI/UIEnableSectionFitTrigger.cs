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
            for (int i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].UITransform)
                {
                    float xMin = 0;
                    float yMin = 0;
                    float xMax = 0;
                    float yMax = 0;
                    if (Sections[i].Left && Sections[i].Left.gameObject.activeInHierarchy)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Sections[i].Left.position, null, out Vector2 xMinOut);
                        xMin = xMinOut.x - (-ScreenCanvas.rect.width * 0.5f);
                    }

                    if (Sections[i].Bottom && Sections[i].Bottom.gameObject.activeInHierarchy)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Sections[i].Bottom.position, null, out Vector2 yMinOut);
                        yMin = yMinOut.y - (-ScreenCanvas.rect.height * 0.5f);
                    }

                    if (Sections[i].Right && Sections[i].Right.gameObject.activeInHierarchy)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Sections[i].Right.position, null, out Vector2 xMaxOut);
                        xMax = xMaxOut.x - (ScreenCanvas.rect.width * 0.5f);
                    }

                    if (Sections[i].Top && Sections[i].Top.gameObject.activeInHierarchy)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Sections[i].Top.position, null, out Vector2 yMaxOut);
                        yMax = yMaxOut.y - (ScreenCanvas.rect.height * 0.5f);
                    }
                    Sections[i].UITransform.offsetMin = new Vector2(xMin, yMin);
                    Sections[i].UITransform.offsetMax = new Vector2(xMax, yMax);
                }
            }
        }

        //void Update()
        //{
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Input.mousePosition, null, out Vector2 xMinOut);
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Input.mousePosition, null, out Vector2 yMinOut);
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Input.mousePosition, null, out Vector2 xMaxOut);
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenCanvas, Input.mousePosition, null, out Vector2 yMaxOut);

        //    float xMin = xMinOut.x - (-ScreenCanvas.rect.width * 0.5f);
        //    float yMin = yMinOut.y - (-ScreenCanvas.rect.height * 0.5f);
        //    float xMax = xMaxOut.x - (ScreenCanvas.rect.width * 0.5f);
        //    float yMax = yMaxOut.y - (ScreenCanvas.rect.height * 0.5f);

        //    Debug.Log($"({xMin}, {yMin}), ({xMax}, {yMax})");
        //}
        #endregion
    }
}
