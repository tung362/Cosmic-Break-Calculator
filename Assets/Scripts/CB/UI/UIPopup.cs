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
    /// Custom Pop-up UI
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class UIPopup : MonoBehaviour, IPointerEnterHandler, ICancelHandler
    {
        /*Events*/
        public UnityEvent OnDeselected;
        public UnityEvent OnCanceled;

        /*Cache*/
        private GameObject Blocker;

        void OnEnable()
        {
            CreateBlocker();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        void OnDisable()
        {
            if (Blocker) Destroy(Blocker);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnCancel(BaseEventData eventData)
        {
            OnCanceled.Invoke();
            gameObject.SetActive(false);
        }

        public void OnDeselect()
        {
            OnDeselected.Invoke();
            gameObject.SetActive(false);
        }

        public void CreateBlocker()
        {
            Canvas canvas = GetComponent<Canvas>();
            GameObject blocker = new GameObject("Blocker");
            RectTransform blockerTransform = blocker.AddComponent<RectTransform>();
            Canvas blockerCanvas = blocker.AddComponent<Canvas>();
            GraphicRaycaster blockerRaycaster = blocker.AddComponent<GraphicRaycaster>();
            Image blockerImage = blocker.AddComponent<Image>();
            Button blockerButton = blocker.AddComponent<Button>();
            blockerTransform.SetParent(canvas.rootCanvas.transform, false);
            blockerTransform.anchorMin = Vector3.zero;
            blockerTransform.anchorMax = Vector3.one;
            blockerTransform.sizeDelta = Vector2.zero;
            blockerCanvas.overrideSorting = true;
            blockerCanvas.sortingOrder = canvas.sortingOrder - 1;
            blockerRaycaster.ignoreReversedGraphics = true;
            blockerImage.color = Color.clear;
            blockerButton.onClick.AddListener(OnDeselect);
            Blocker = blocker;
        }
    }
}
