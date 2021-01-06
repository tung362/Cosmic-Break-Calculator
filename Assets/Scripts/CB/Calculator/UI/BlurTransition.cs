using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI effects for transitioning in and out of blur, non animated
    /// </summary>
    public class BlurTransition : MonoBehaviour
    {
        public Image BlurBind;
        public float MinBlurRadius = 0.0f;
        public float MaxBlurRadius = 4.0f;
        public float DurationIn = 1.0f;
        public float DurationOut = 1.0f;

        /*Events*/
        public UnityEvent OnTransitionIn = new UnityEvent();
        public UnityEvent OnTransitionOut = new UnityEvent();

        /*Cache*/
        private bool TransitionIn = false;
        private float CurrentVelocity = 0;

        void Start()
        {

        }

        void Update()
        {
            float currentRadius = BlurBind.material.GetFloat("_Radius");
            float smooth;
            if (TransitionIn)
            {
                smooth = Mathf.Clamp(Mathf.SmoothDamp(currentRadius, MaxBlurRadius, ref CurrentVelocity, DurationIn), MinBlurRadius, MaxBlurRadius);
            }
            else
            {
                smooth = Mathf.Clamp(Mathf.SmoothDamp(currentRadius, MinBlurRadius, ref CurrentVelocity, DurationOut), MinBlurRadius, MaxBlurRadius);
            }
            BlurBind.material.SetFloat("_Radius", smooth);

            if (smooth <= MinBlurRadius + 0.01f) OnTransitionOut.Invoke();
            if (smooth >= MaxBlurRadius - 0.01f) OnTransitionIn.Invoke();
        }

        public void TransitionToggle(bool toggle)
        {
            TransitionIn = toggle;
        }
    }
}
