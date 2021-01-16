using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// UI trigger for invoking events with slider
    /// </summary>
    public class UISliderEvent : MonoBehaviour, IEndDragHandler, IPointerUpHandler
    {
        public Slider SliderBind;

        /*Events*/
        public FloatEvent OnClickUp;
        public FloatEvent OnDragEnd;

        public void OnPointerUp(PointerEventData eventData)
        {
            OnClickUp.Invoke(SliderBind.value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnd.Invoke(SliderBind.value);
        }
    }
}
