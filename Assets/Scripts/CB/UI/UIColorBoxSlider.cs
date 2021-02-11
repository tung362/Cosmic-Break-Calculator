using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom box slider UI for Color type
    /// </summary>
    public class UIColorBoxSlider : MonoBehaviour
    {
        public UIColorPicker ColorPickerBind;
        public UIBoxSlider BoxSliderBind;
        public RawImage ImageBind;

        /*Cache*/
        private float previousH = 0;

        void OnEnable()
        {
            //Set listener
            if (ColorPickerBind) ColorPickerBind.OnValueChangedInteral.AddListener(OnValueChangedInteral);
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
            if (!previousH.Equals(ColorPickerBind.H))
            {
                previousH = ColorPickerBind.H;
                RegenerateTexture();
            }

            Vector2 sv = BoxSliderBind.NormalizedValue;
            if (!ColorPickerBind.S.Equals(BoxSliderBind.NormalizedValue.x)) sv.x = ColorPickerBind.S;
            if (!ColorPickerBind.V.Equals(BoxSliderBind.NormalizedValue.y)) sv.y = ColorPickerBind.V;
            BoxSliderBind.NormalizedValue = sv;
        }
        #endregion

        #region Utils
        public void SetColorPickerValue(Vector2 sv)
        {
            ColorPickerBind.SetValue(sv.x, UIColorSlider.ColorType.S);
            ColorPickerBind.SetValue(sv.y, UIColorSlider.ColorType.V);
        }
        #endregion

        #region Functions
        void RegenerateTexture()
        {
            if (ImageBind.texture != null) DestroyImmediate(ImageBind.texture);

            double h = 0;
            if(ColorPickerBind) h = ColorPickerBind.H * 360;

            var texture = new Texture2D(128, 128);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.hideFlags = HideFlags.DontSave;

            for (int s = 0; s < 128; s++)
            {
                Color[] colors = new Color[128];
                for (int v = 0; v < 128; v++) colors[v] = ColorConversion.ConvertHSVToRGB(h, s / 128.0f, v / 128.0f, 1);
                texture.SetPixels(s, 0, 1, 128, colors);
            }
            texture.Apply();
            ImageBind.texture = texture;
        }
        #endregion
    }
}
