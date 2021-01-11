using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CB.UI
{
    /// <summary>
    /// UI trigger for invoking events when slider dragging ends
    /// </summary>
    public class UISliderDragEndEvent : MonoBehaviour, IEndDragHandler
    {
        #region Format
        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { }
        #endregion

        public Slider SliderBind;

        /*Events*/
        public FloatEvent OnDragEnd;

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnd.Invoke(SliderBind.value);
        }
    }
}
