using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Represents the assembled data of a generic robot or part of a generic robot
    /// </summary>
    //[System.Serializable]
    [MessagePackObject]
    public class Contraption
    {
        [Key(0)]
        public int MaxLevel = 10;
        [Key(1)]
        public int ExTuneCount = 0;
        [Key(2)]
        public Stats TotalStats = new Stats();
        [Key(3)]
        public List<(Cartridge, int, bool)> Cartridges = new List<(Cartridge, int, bool)>();
        [Key(4)]
        public PartJoint Root = new PartJoint();
    }
}