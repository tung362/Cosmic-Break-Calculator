using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom color picker UI
    /// </summary>
    public class UIColorPicker : MonoBehaviour
    {
        /*Enums*/
        public enum ColorModeType { RGB255, RGB1, HSV }

        public Image ColorPreview;
        public TMP_Dropdown ColorModeDropdown;
        public TMP_InputField HexInputField;
        public RectTransform R255Slider;
        public RectTransform G255Slider;
        public RectTransform B255Slider;
        public RectTransform R1Slider;
        public RectTransform G1Slider;
        public RectTransform B1Slider;
        public RectTransform HSlider;
        public RectTransform SSlider;
        public RectTransform VSlider;
        public ColorModeType ColorMode = ColorModeType.HSV;

        /*Events*/
        public ColorEvent OnValueChanged;
        [HideInInspector]
        public ColorEvent OnValueChangedInteral;

        /*Cache*/
        private Color _Value = Color.black;
        public Color Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                UpdatePreview();
                OnValueChangedInteral.Invoke(Value);
                OnValueChanged.Invoke(Value);
            }
        }
        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }

        #region Utils
        public void SetValue(Color color)
        {
            _Value = color;
            SetHSV();
            UpdatePreview();
            UpdateHexadecimal(Value);
            OnValueChangedInteral.Invoke(Value);
            OnValueChanged.Invoke(Value);
        }

        public void SetValue(float num, UIColorSlider.ColorType colorType)
        {
            SetValueByColorType(num, colorType);
            UpdatePreview();
            UpdateHexadecimal(Value);
            OnValueChangedInteral.Invoke(Value);
            OnValueChanged.Invoke(Value);
        }

        public void SetValue(string hex)
        {
            ColorUtility.TryParseHtmlString("#" + hex, out Color color);
            SetValue(color);
        }

        public void SetValueNoCallback(Color color)
        {
            _Value = color;
            SetHSV();
            UpdatePreview();
            UpdateHexadecimal(Value);
            OnValueChangedInteral.Invoke(Value);
        }

        public void SetValueNoCallback(float num, UIColorSlider.ColorType colorType)
        {
            SetValueByColorType(num, colorType);
            UpdatePreview();
            UpdateHexadecimal(Value);
            OnValueChangedInteral.Invoke(Value);
        }

        public void SetValueNoCallback(string hex)
        {
            ColorUtility.TryParseHtmlString("#" + hex, out Color color);
            SetValueNoCallback(color);
        }

        public void SetColorMode(int num)
        {
            ColorMode = (ColorModeType)num;
            R255Slider.gameObject.SetActive(false);
            G255Slider.gameObject.SetActive(false);
            B255Slider.gameObject.SetActive(false);
            R1Slider.gameObject.SetActive(false);
            G1Slider.gameObject.SetActive(false);
            B1Slider.gameObject.SetActive(false);
            HSlider.gameObject.SetActive(false);
            SSlider.gameObject.SetActive(false);
            VSlider.gameObject.SetActive(false);

            switch (ColorMode)
            {
                case ColorModeType.RGB255:
                    R255Slider.gameObject.SetActive(true);
                    G255Slider.gameObject.SetActive(true);
                    B255Slider.gameObject.SetActive(true);
                    break;
                case ColorModeType.RGB1:
                    R1Slider.gameObject.SetActive(true);
                    G1Slider.gameObject.SetActive(true);
                    B1Slider.gameObject.SetActive(true);
                    break;
                case ColorModeType.HSV:
                    HSlider.gameObject.SetActive(true);
                    SSlider.gameObject.SetActive(true);
                    VSlider.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Functions
        void SetValueByColorType(float num, UIColorSlider.ColorType colorType)
        {
            switch (colorType)
            {
                case UIColorSlider.ColorType.R:
                    if(_Value.r != num)
                    {
                        _Value.r = num;
                        SetHSV();
                    }
                    break;
                case UIColorSlider.ColorType.G:
                    if (_Value.g != num)
                    {
                        _Value.g = num;
                        SetHSV();
                    }
                    break;
                case UIColorSlider.ColorType.B:
                    if (_Value.b != num)
                    {
                        _Value.b = num;
                        SetHSV();
                    }
                    break;
                case UIColorSlider.ColorType.A:
                    if (_Value.a != num) _Value.a = num;
                    break;
                case UIColorSlider.ColorType.H:
                    if (H != num)
                    {
                        H = num;
                        SetRGB();
                    }
                    break;
                case UIColorSlider.ColorType.S:
                    if (S != num)
                    {
                        S = num;
                        SetRGB();
                    }
                    break;
                case UIColorSlider.ColorType.V:
                    if (V != num)
                    {
                        V = num;
                        SetRGB();
                    }
                    break;
                default:
                    break;
            }
        }

        void UpdatePreview()
        {
            ColorPreview.color = _Value;
        }

        void UpdateHexadecimal(Color color)
        {
            HexInputField.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGBA(color));
        }

        void SetHSV()
        {
            ColorConversion.HSVColor hsvColor = ColorConversion.ConvertRGBToHSV(_Value);
            H = hsvColor.NormalizedH;
            S = hsvColor.NormalizedS;
            V = hsvColor.NormalizedV;
        }

        void SetRGB()
        {
            _Value = ColorConversion.ConvertHSVToRGB(H * 360, S, V, _Value.a);
        }
        #endregion
    }
}
