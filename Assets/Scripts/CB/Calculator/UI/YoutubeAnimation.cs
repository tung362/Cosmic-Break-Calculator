using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI animation for transitioning in and out of youtube tracks
    /// </summary>
    public class YoutubeAnimation : MonoBehaviour
    {
        public Animator AnimatorBind;
        [Range(0.0f, 1.0f)] public float Step = 0.0f;
        public bool UpdateAnimation = false;

        /*Events*/
        public UnityEvent OnTransitionIn = new UnityEvent();
        public UnityEvent OnTransitionOut = new UnityEvent();

        /*Cache*/
        private bool IsIn = false; //Prevents spam

        void Update()
        {
            if (UpdateAnimation) Calculator.instance.VideoPlayerMaterial.SetFloat("_Step", Step);
        }

        public void SetStep(int step)
        {
            Step = step;
            Calculator.instance.VideoPlayerMaterial.SetFloat("_Step", Step);
        }

        public void BeginTransition(bool transitionIn)
        {
            if (IsIn == transitionIn) return;
            AnimatorBind.SetBool("TransitionIn", transitionIn);
            AnimatorBind.SetTrigger("Transition");
            IsIn = !IsIn;
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
