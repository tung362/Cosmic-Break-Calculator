﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// Event handler for window resize
    /// </summary>
    //[ExecuteInEditMode]
    public class WindowResizeEvent : MonoBehaviour
    {
        public static WindowResizeEvent instance { get; private set; }

        /*Callbacks*/
        public static event Action OnWindowResize;

        /*Cache*/
        private Vector2 PreviousResolution = new Vector2(Screen.width, Screen.height);
        private bool RequestedInvoke = false;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.LogWarning("Warning! Multiple instances of \"WindowResizeEvent\"");
        }

        void LateUpdate()
        {
            //Check if window was resized
            if (PreviousResolution.x != Screen.width || PreviousResolution.y != Screen.height || RequestedInvoke)
            {
                PreviousResolution.x = Screen.width;
                PreviousResolution.y = Screen.height;
                //Event callback
                OnWindowResize?.Invoke();
                RequestedInvoke = false;
            }
        }

        public void RequestOnWindowResize()
        {
            RequestedInvoke = true;
        }
    }
}
