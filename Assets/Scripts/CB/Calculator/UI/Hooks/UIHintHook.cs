using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Hook for sending updates to the UIHintBind
    /// UIHintBind must be set up
    /// </summary>
    public class UIHintHook : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string EnterText = "";
        public UIHintHook ExitChain;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            UIHintBind.instance.UpdateText(EnterText);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            UIHintBind.instance.UpdateText(ExitChain ? ExitChain.EnterText : "");
        }
    }
}
