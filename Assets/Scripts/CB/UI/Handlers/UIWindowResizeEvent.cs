using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CB.UI
{
    /// <summary>
    /// UI effect for invoking events when window is resized
    /// </summary>
    public class UIWindowResizeEvent : MonoBehaviour
    {
        public UnityEvent OnResize;

        void OnEnable()
        {
            //Set listener
            WindowResizeEvent.OnWindowResize += OnWindowResize;
            OnWindowResize();
        }

        void OnDisable()
        {
            //Unset listener
            WindowResizeEvent.OnWindowResize -= OnWindowResize;
        }

        void OnWindowResize()
        {
            OnResize.Invoke();
        }
    }
}
