using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom box slider UI
    /// </summary>
    public class UIBoxSlider : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
    {
        public RectTransform HandleContainer;
        public RectTransform HandleBind;
        public float MinValue = 0.0f;
        public float MaxValue = 1.0f;
        public bool WholeNumbers = false;

        /*Events*/
        public Vector2Event OnValueChanged;

        /*Cache*/
        private Vector2 _Value = Vector2.zero;
        public Vector2 Value
        {
            get
            {
                if (WholeNumbers) return new Vector2(Mathf.Round(_Value.x), Mathf.Round(_Value.y));
                return _Value;
            }
            set
            {
                Vector2 clampedValue = new Vector2(Mathf.Clamp(value.x, MinValue, MaxValue), Mathf.Clamp(value.y, MinValue, MaxValue));
                if (WholeNumbers) clampedValue = new Vector2(Mathf.Round(clampedValue.x), Mathf.Round(clampedValue.y));

                if (_Value.Equals(clampedValue)) return;

                _Value = clampedValue;
                UpdateHandle();
                OnValueChanged.Invoke(Value);
            }
        }

        public Vector2 NormalizedValue
        {
            get
            {
                if (Mathf.Approximately(MinValue, MaxValue)) return Vector2.zero;
                return new Vector2(Mathf.InverseLerp(MinValue, MaxValue, Value.x), Mathf.InverseLerp(MinValue, MaxValue, Value.y));
            }
            set
            {
                Value = new Vector2(Mathf.Lerp(MinValue, MaxValue, value.x), Mathf.Lerp(MinValue, MaxValue, value.y));
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        #region Utils
        public void SetValue(Vector2 pos)
        {
            Vector2 clampedValue = new Vector2(Mathf.Clamp(pos.x, MinValue, MaxValue), Mathf.Clamp(pos.y, MinValue, MaxValue));
            if (WholeNumbers) clampedValue = new Vector2(Mathf.Round(clampedValue.x), Mathf.Round(clampedValue.y));

            if (_Value.Equals(clampedValue)) return;

            _Value = clampedValue;
            UpdateHandle();
            OnValueChanged.Invoke(Value);
        }

        public void SetValueNoCallback(Vector2 pos)
        {
            Vector2 clampedValue = new Vector2(Mathf.Clamp(pos.x, MinValue, MaxValue), Mathf.Clamp(pos.y, MinValue, MaxValue));
            if (WholeNumbers) clampedValue = new Vector2(Mathf.Round(clampedValue.x), Mathf.Round(clampedValue.y));

            if (_Value.Equals(clampedValue)) return;

            _Value = clampedValue;
            UpdateHandle();
        }
        #endregion

        #region Functions
        void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            if (HandleContainer.rect.size.x > 0)
            {
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(HandleContainer, eventData.position, cam, out Vector2 localCursor)) return;
                localCursor -= HandleContainer.rect.position;

                float normalizedX = Mathf.Clamp01(localCursor.x / HandleContainer.rect.size.x);
                float normalizedY = Mathf.Clamp01(localCursor.y / HandleContainer.rect.size.y);
                NormalizedValue = new Vector2(normalizedX, normalizedY);
            }
        }

        void UpdateHandle()
        {
            HandleBind.anchorMin = NormalizedValue;
            HandleBind.anchorMax = NormalizedValue;
        }
        #endregion
    }
}
