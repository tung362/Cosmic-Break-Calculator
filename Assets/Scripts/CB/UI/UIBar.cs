using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// Custom bar UI
    /// </summary>
    public class UIBar : MonoBehaviour
    {
        public RectTransform BarBind;
        public bool Vertical = false;
        public Vector2 MaxSize = new Vector2(248.0f, 5.0f);
        public bool SmoothTransition = false;
        public float Smoothness = 0.3f;

        /*Cache*/
        private float CurrentProgress = 0;
        private float SmoothVelocity = 0;

        public void UpdateProgress(float progress)
        {
            CurrentProgress = progress;
            if (!SmoothTransition) CalculateBar();
        }

        void Update()
        {
            if (SmoothTransition) CalculateBar();
        }

        public void CalculateBar()
        {
            Vector2 Movement = BarBind.sizeDelta;
            if (Vertical)
            {
                float change = SmoothTransition ? Mathf.SmoothDamp(Movement.y, MaxSize.y * Mathf.Clamp(CurrentProgress, 0, 1), ref SmoothVelocity, Smoothness, Mathf.Infinity, Time.deltaTime) : MaxSize.y * Mathf.Clamp(CurrentProgress, 0, 1);
                Movement.y = change;
            }
            else
            {
                float change = SmoothTransition ? Mathf.SmoothDamp(Movement.x, MaxSize.x * Mathf.Clamp(CurrentProgress, 0, 1), ref SmoothVelocity, Smoothness, Mathf.Infinity, Time.deltaTime) : MaxSize.x * Mathf.Clamp(CurrentProgress, 0, 1);
                Movement.x = change;
            }
            BarBind.sizeDelta = Movement;
        }
    }
}
