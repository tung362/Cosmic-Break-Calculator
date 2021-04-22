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
        #region Format
        public class StateParams
        {
            public bool TransitionIn;
        }
        #endregion

        public Animator AnimatorBind;
        public Image ImageBind;
        public float Radius = 0.0f;
        public bool UpdateAnimation = false;

        /*Events*/
        public UnityEvent OnTransitionIn = new UnityEvent();
        public UnityEvent OnTransitionOut = new UnityEvent();

        /*Cache*/
        private Material BlurInstance;
        private StateParams StateQueue; //The state to take when re-enabled if there was a state change request when disabled

        void OnEnable()
        {
            if(StateQueue != null)
            {
                BeginTransition(StateQueue.TransitionIn);
                StateQueue = null;
            }
        }

        void Start()
        {
            if(!BlurInstance)
            {
                BlurInstance = new Material(Calculator.instance.BlurMaterial);
                ImageBind.material = BlurInstance;
            }
            AnimatorBind.keepAnimatorControllerStateOnDisable = true;
        }

        void Update()
        {
            if (UpdateAnimation) BlurInstance.SetFloat("_Radius", Radius);
        }

        void OnDestroy()
        {
            Destroy(BlurInstance);
        }

        public void SetRadius(int radius)
        {
            Radius = radius;
            BlurInstance.SetFloat("_Radius", Radius);
        }

        public void BeginTransition(bool transitionIn)
        {
            if(!gameObject.activeInHierarchy)
            {
                StateQueue = new StateParams
                {
                    TransitionIn = transitionIn
                };
            }
            else
            {
                AnimatorBind.SetBool("TransitionIn", transitionIn);
                AnimatorBind.SetTrigger("Transition");
            }
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
