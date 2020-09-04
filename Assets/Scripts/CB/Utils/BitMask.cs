using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Utils
{
    /// <summary>
    /// Mask for storing multiple flags using bit shifting
    /// </summary>
    [System.Serializable]
    public struct Bitmask
    {
        public int Mask { get; private set; }

        public bool HasFlag(int flag)
        {
            return ((int)Mask & 1 << (int)flag) != 0;
        }

        public void AddFlag(int flag)
        {
            Mask |= (int)(1 << (int)flag);
        }

        public void RemoveFlag(int flag)
        {
            Mask &= (int)(~(int)(1 << (int)flag));
        }

        public void ClearFlags()
        {
            Mask = 0;
        }
    }
}
