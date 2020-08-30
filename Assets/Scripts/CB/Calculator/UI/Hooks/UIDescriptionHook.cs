using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Hook for dynamically generated game objects to update to the UIDescriptionHook
    /// UIDescriptionHook must be set up
    /// </summary>
    public class UIDescriptionHook : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UIDescriptionModule.IUIDescription DescriptionObject;
        public string ExitText = "";

        private UIDescriptionModule.IUIDescription DescriptionInterface => DescriptionObject as UIDescriptionModule.IUIDescription;

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIHintModule.instance.UpdateText(DescriptionInterface.GetDescription());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIHintModule.instance.UpdateText(ExitText);
        }
    }
}
