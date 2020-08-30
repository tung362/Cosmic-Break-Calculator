using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Utils
{
    [System.Serializable]
    public class BitMask
    {
        public int Mask { get; private set; }

        bool HasFlag(int flag)
        {
            return ((int)Mask & 1 << (int)flag) != 0;
        }

        void AddFlag(int flag)
        {
            Mask |= (int)(1 << (int)flag);
        }

        void RemoveFlag(int flag)
        {
            Mask &= (int)(~(int)(1 << (int)flag));
        }
    }
}
