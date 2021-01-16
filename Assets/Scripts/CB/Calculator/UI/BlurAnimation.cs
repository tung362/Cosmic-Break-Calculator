using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI animation for transitioning in and out of blur
    /// </summary>
    public class BlurAnimation : MonoBehaviour
    {
        public Animator AnimatorBind;
        public float Radius = 0.0f;
        public bool UpdateAnimation = false;

        /*Events*/
        public UnityEvent OnTransitionIn = new UnityEvent();
        public UnityEvent OnTransitionOut = new UnityEvent();

        void Update()
        {
            if (UpdateAnimation) Calculator.instance.BlurMaterial.SetFloat("_Radius", Radius);
        }

        public void SetRadius(int radius)
        {
            Radius = radius;
            Calculator.instance.BlurMaterial.SetFloat("_Radius", Radius);
        }

        public void BeginTransition(bool transitionIn)
        {
            AnimatorBind.SetBool("TransitionIn", transitionIn);
            AnimatorBind.SetTrigger("Transition");
        }

        public void InvokeOnTransitionIn()
        {
            OnTransitionIn.Invoke();
        }

        public void InvokeOnTransitionOut()
        {
            OnTransitionOut.Invoke();
        }
    }
}
