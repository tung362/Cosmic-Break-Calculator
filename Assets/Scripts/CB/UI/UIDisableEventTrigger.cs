using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CB.UI
{
    /// <summary>
    /// UI trigger for invoking events when disabled
    /// </summary>
    [ExecuteInEditMode]
    public class UIDisableEventTrigger : MonoBehaviour
    {
        public UnityEvent Events = new UnityEvent();

        void OnDisable()
        {
            Events.Invoke();
        }
    }
}
