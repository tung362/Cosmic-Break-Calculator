using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Calculator option settings
    /// </summary>
    [MessagePackObject]
    public class Options
    {
        /*Enums*/
        public enum LanguageType { EN, JP, CH };

        /*Builder*/
        [Key(0)]
        public float ScaleFactor = 1.0f;

        [Key(1)]
        public int FilesLoadedPerChunk = 64;

        [Key(2)]
        public LanguageType LanguageMode = LanguageType.EN;

        /*Audio*/
        [Key(3)]
        public bool AudioVisualizer = false;

        [Key(4)]
        public float Sound = 0.25f;

        /*Video*/
        [Key(5)]
        public bool VideoBackground = false;

        [Key(6)]
        public bool Grayscale = true;
    }
}
