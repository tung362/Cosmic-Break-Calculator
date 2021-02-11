using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CB.Utils;
using CB.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI tune color palette editor
    /// </summary>
    public class TunePaletteEdit : MonoBehaviour
    {
        public UIColorDropdown ColorDropdownBind;

        void Start()
        {
            Load();
        }

        public void Save()
        {
            Calculator.instance.TuneColorPalette.ColorPalette = ColorDropdownBind.Options;
            Calculator.instance.SaveTunePalette();
        }

        public void Load()
        {
            ColorDropdownBind.Options = Calculator.instance.TuneColorPalette.ColorPalette;
        }
    }
}
