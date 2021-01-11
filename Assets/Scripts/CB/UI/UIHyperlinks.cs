using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace CB.UI
{
    /// <summary>
    /// Opens hyperlinks when clicked on
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class UIHyperlinks : MonoBehaviour, IPointerClickHandler
    {
        public TMP_Text TextBind;

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(TextBind, Input.mousePosition, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = TextBind.textInfo.linkInfo[linkIndex];

                //Open link
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}
