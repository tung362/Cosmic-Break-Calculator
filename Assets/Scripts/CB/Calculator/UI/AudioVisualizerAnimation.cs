using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI animation for transitioning in and out of the audio visualizer
    /// </summary>
    public class AudioVisualizerAnimation : MonoBehaviour
    {
        public TMP_InputField InputFieldBind;
        public Animator AnimatorBind;
        [Range(0.0f, 1.0f)] public float Step = 0.0f;
        public bool UpdateAnimation = false;
        public float SelectionAlpha = 0.75f;
        public float TextAlpha = 1.0f;

        /*Events*/
        public UnityEvent OnTransitionIn = new UnityEvent();
        public UnityEvent OnTransitionOut = new UnityEvent();

        /*Cache*/
        private bool IsIn = false; //Prevents spam

        void Update()
        {
            if (UpdateAnimation)
            {
                Color selectionColor = InputFieldBind.selectionColor;
                Color textColor = InputFieldBind.textComponent.color;
                Calculator.instance.AudioVisualizerMaterial.SetFloat("_Step", Step);
                InputFieldBind.selectionColor = new Color(selectionColor.r, selectionColor.g, selectionColor.b, Mathf.Lerp(0, SelectionAlpha, Step));
                InputFieldBind.textComponent.color = new Color(textColor.r, textColor.g, textColor.b, Mathf.Lerp(0, TextAlpha, Step));
            }
        }

        public void SetStep(int step)
        {
            Color selectionColor = InputFieldBind.selectionColor;
            Color textColor = InputFieldBind.textComponent.color;
            Step = step;
            Calculator.instance.AudioVisualizerMaterial.SetFloat("_Step", Step);
            InputFieldBind.selectionColor = new Color(selectionColor.r, selectionColor.g, selectionColor.b, Mathf.Lerp(0, SelectionAlpha, Step));
            InputFieldBind.textComponent.color = new Color(textColor.r, textColor.g, textColor.b, Mathf.Lerp(0, TextAlpha, Step));
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
