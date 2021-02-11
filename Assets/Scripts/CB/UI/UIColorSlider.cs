using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom slider UI for Color type
    /// </summary>
    public class UIColorSlider : MonoBehaviour
    {
        /*Enums*/
        public enum ColorType { R, G, B, A, H, S, V }

        public UIColorPicker ColorPickerBind;
        public Slider SliderBind;
        public RawImage ImageBind;
        public TMP_InputField ValueInputField;
        public bool NormalizedRange = false;
        public ColorType ColorValue;

        void OnEnable()
        {
            if (ColorPickerBind)
            {
                //Load
                OnValueChangedInteral(ColorPickerBind.Value);
                //Set listener
                ColorPickerBind.OnValueChangedInteral.AddListener(OnValueChangedInteral);
            }
        }

        void OnDisable()
        {
            //Unset listener
            if (ColorPickerBind) ColorPickerBind.OnValueChangedInteral.RemoveListener(OnValueChangedInteral);
        }

        void Start()
        {
            RegenerateTexture();
        }

        void OnDestroy()
        {
            if (ImageBind.texture != null) DestroyImmediate(ImageBind.texture);
        }

        #region Listeners
        void OnValueChangedInteral(Color color)
        {
            switch (ColorValue)
            {
                case ColorType.R:
                    SliderBind.normalizedValue = color.r;
                    RegenerateTexture();
                    break;
                case ColorType.G:
                    SliderBind.normalizedValue = color.g;
                    RegenerateTexture();
                    break;
                case ColorType.B:
                    SliderBind.normalizedValue = color.b;
                    RegenerateTexture();
                    break;
                case ColorType.A:
                    SliderBind.normalizedValue = color.a;
                    break;
                case ColorType.H:
                    SliderBind.normalizedValue = ColorPickerBind.H;
                    break;
                case ColorType.S:
                    SliderBind.normalizedValue = ColorPickerBind.S;
                    RegenerateTexture();
                    break;
                case ColorType.V:
                    SliderBind.normalizedValue = ColorPickerBind.V;
                    RegenerateTexture();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Utils
        public void SetColorPickerValue(float num)
        {
            if (ColorPickerBind) ColorPickerBind.SetValue(num, ColorValue);
            if (ValueInputField)
            {
                if (NormalizedRange) ValueInputField.SetTextWithoutNotify(num.ToString());
                else ValueInputField.SetTextWithoutNotify(Mathf.FloorToInt(num * 255.0f).ToString());
            }
        }

        public void SetColorPickerValue(string text)
        {
            float.TryParse(text, out float value);
            if (ColorPickerBind)
            {
                if (NormalizedRange) ColorPickerBind.SetValue(value, ColorValue);
                else ColorPickerBind.SetValue(value / 255.0f, ColorValue);
            }
        }
        #endregion

        #region Functions
        void RegenerateTexture()
        {
            if (ImageBind.texture != null) DestroyImmediate(ImageBind.texture);

            Color32 baseColor = Color.black;
            float h = 0;
            float s = 0;
            float v = 0;
            bool vertical = SliderBind.direction == Slider.Direction.BottomToTop || SliderBind.direction == Slider.Direction.TopToBottom;
            bool inverted = SliderBind.direction == Slider.Direction.TopToBottom || SliderBind.direction == Slider.Direction.RightToLeft;

            if (ColorPickerBind)
            {
                baseColor = ColorPickerBind.Value;
                h = ColorPickerBind.H;
                s = ColorPickerBind.S;
                v = ColorPickerBind.V;
            }

            int size = 255;
            switch (ColorValue)
            {
                case ColorType.H:
                    size = 360;
                    break;
                case ColorType.S:
                case ColorType.V:
                    size = 100;
                    break;
                default:
                    break;
            }

            Color32[] colors = new Color32[size];
            Texture2D texture = vertical ? new Texture2D(1, size) : new Texture2D(size, 1);
            texture.hideFlags = HideFlags.DontSave;

            switch (ColorValue)
            {
                case ColorType.R:
                    for (byte i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = new Color32(i, baseColor.g, baseColor.b, 255);
                    break;
                case ColorType.G:
                    for (byte i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, i, baseColor.b, 255);
                    break;
                case ColorType.B:
                    for (byte i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = new Color32(baseColor.r, baseColor.g, i, 255);
                    break;
                case ColorType.A:
                    for (byte i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = new Color32(i, i, i, 255);
                    break;
                case ColorType.H:
                    for (int i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = ColorConversion.ConvertHSVToRGB(i, 1, 1, 1);
                    break;
                case ColorType.S:
                    for (int i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = ColorConversion.ConvertHSVToRGB(h * 360, (float)i / size, v, 1);
                    break;
                case ColorType.V:
                    for (int i = 0; i < size; i++) colors[inverted ? size - 1 - i : i] = ColorConversion.ConvertHSVToRGB(h * 360, s, (float)i / size, 1);
                    break;
                default:
                    break;
            }
            texture.SetPixels32(colors);
            texture.Apply();
            ImageBind.texture = texture;

            switch (SliderBind.direction)
            {
                case Slider.Direction.BottomToTop:
                case Slider.Direction.TopToBottom:
                    ImageBind.uvRect = new Rect(0, 0, 2, 1);
                    break;
                case Slider.Direction.LeftToRight:
                case Slider.Direction.RightToLeft:
                    ImageBind.uvRect = new Rect(0, 0, 1, 2);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
