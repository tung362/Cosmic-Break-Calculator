using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Hook for dynamically generated game objects to update to the UIHintModule
    /// UIHintModule must be set up
    /// </summary>
    public class UIHintHook : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string EnterText = "";
        public string ExitText = "";

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIHintModule.instance.UpdateText(EnterText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIHintModule.instance.UpdateText(ExitText);
        }
    }
}
