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
        /*UI*/
        [Key(0)]
        public float ScaleFactor = 1.0f;

        /*Audio*/
        [Key(1)]
        public bool AudioVisualizer = false;
        [Key(2)]
        public float Sound = 0.5f;

        /*Video*/
        [Key(3)]
        public bool VideoBackground = false;
    }
}
