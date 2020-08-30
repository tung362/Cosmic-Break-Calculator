using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CB.UI
{
    /// <summary>
    /// UI trigger for invoking events when enabled
    /// </summary>
    [ExecuteInEditMode]
    public class UIEnableEventTrigger : MonoBehaviour
    {
        public UnityEvent Events = new UnityEvent();

        void OnEnable()
        {
            Events.Invoke();
        }
    }
}
