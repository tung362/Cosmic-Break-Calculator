using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// UI effects for counting up and down
    /// </summary>
    public class UICount : MonoBehaviour
    {
        public int Count = 0;
        public bool Repeat = true;
        public Vector2Int RepeatRange = Vector2Int.zero;

        /*Events*/
        public IntEvent OnCountChange;

        public void CountUp()
        {
            if (Repeat) Count = Count < RepeatRange.y ? Count + 1 : RepeatRange.x;
            else Count += 1;
            OnCountChange.Invoke(Count);
        }

        public void CountDown()
        {
            if (Repeat) Count = Count > RepeatRange.x ? Count - 1 : RepeatRange.y;
            else Count -= 1;
            OnCountChange.Invoke(Count);
        }
    }
}
