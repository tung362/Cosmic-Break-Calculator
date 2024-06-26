﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace CB.UI
{
    /// <summary>
    /// Enforces numbers to follow certain rules in input fields
    /// </summary>
    public class UINumberRestriction : MonoBehaviour
    {
        public TMP_InputField Bind;
        public float MinValue;
        public float MaxValue;

        /*Events*/
        public UnityEvent OnMinReached = new UnityEvent();
        public UnityEvent OnMaxReached = new UnityEvent();
        public UnityEvent OnInBetween = new UnityEvent();

        public void RestrictNegative(string text)
        {
            if (text.Length > 0 && text[0] == '-') Bind.SetTextWithoutNotify("0");
        }

        public void RestrictGreaterThan(string text)
        {
            float.TryParse(text, out float value);
            if (value > MaxValue) Bind.SetTextWithoutNotify(MaxValue.ToString());
        }

        public void RestrictLessThan(string text)
        {
            float.TryParse(text, out float value);
            if (value < MinValue) Bind.SetTextWithoutNotify(MinValue.ToString());
        }

        public void EnsureNumber(string text)
        {
            float.TryParse(text, out float value);
            Bind.SetTextWithoutNotify(value.ToString());
        }

        public void NumberEvent(string text)
        {
            float.TryParse(text, out float value);
            if (value > MinValue && value < MaxValue) OnInBetween.Invoke();
            if (value >= MaxValue) OnMaxReached.Invoke();
            if (value <= MinValue) OnMinReached.Invoke();
        }
    }
}
