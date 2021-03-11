using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom draggable resize UI
    /// </summary>
    public class UIResizeDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform Bind;
        public bool ResizeX = false;
        public bool ResizeY = false;
        public float MinSizeX = 30.0f;
        public float MinSizeY = 30.0f;

        public UnityEvent OnResizeBegin;
        public Vector2Event OnResize;
        public UnityEvent OnResizeEnd;

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnResizeBegin.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Bind, Input.mousePosition, null, out Vector2 mousePosition);
            Vector2 resize = Bind.sizeDelta;
            if (ResizeX) resize.x = mousePosition.x >= MinSizeX ? mousePosition.x : MinSizeX;
            if (ResizeY) resize.y = -mousePosition.y >= MinSizeY ? -mousePosition.y : MinSizeY;
            Bind.sizeDelta = resize;
            OnResize.Invoke(resize);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnResizeEnd.Invoke();
        }
    }
}
