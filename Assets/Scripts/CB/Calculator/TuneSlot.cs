using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of Tune
    /// </summary>
    public class TuneSlot : MonoBehaviour
    {
        //Hook to an existing Tune
        public Tune Slot;

        [Header("Binds")]
        public Image Icon;
    }
}
