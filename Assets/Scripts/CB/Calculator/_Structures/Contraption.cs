using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Calculator
{
    /// <summary>
    /// Represents the assembled data of a generic robot or part of a generic robot
    /// </summary>
    //[System.Serializable]
    public class Contraption
    {
        public int MaxLevel = 10;
        public int ExTuneCount = 0;
        public Stats TotalStats = new Stats();
        public List<(Cartridge, bool)> Cartridges = new List<(Cartridge, bool)>();
        public Part Root = new Part();
    }
}