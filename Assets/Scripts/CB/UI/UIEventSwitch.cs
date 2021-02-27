using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching events
    /// </summary>
    public class UIEventSwitch : MonoBehaviour
    {
        public UnityEvent OnEvent;
        public UnityEvent OffEvent;

        public void SwitchEvent(bool toggle)
        {
            if (toggle) OnEvent.Invoke();
            else OffEvent.Invoke();
        }
    }
}
