using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Calculator
{
    [System.Serializable]
    public class Bot
    {
        public int MaxLevel = 10;
        public int ExTuneCount = 0;
        public Stats BaseStats = new Stats();
        public List<Cartridge> Carts = new List<Cartridge>();
        public PartJoint JointRoot = null;
    }
}
