using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Calculator.Utils;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Tune color Palette
    /// </summary>
    [MessagePackObject]
    public class TunePalette
    {
        [Key(0)]
        public List<Color> ColorPalette = new List<Color>
        {
            { DefaultColors.RegularTune },
            { DefaultColors.ExTune },
            { DefaultColors.MAIN },
            { DefaultColors.SUB }
        };
    }
}
