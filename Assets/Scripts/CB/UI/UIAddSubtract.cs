using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CB.UI
{
    /// <summary>
    /// UI effects for adding and subtracting
    /// </summary>
    public class UIAddSubtract : MonoBehaviour
    {
        public TMP_InputField Bind;

        public void Add(int num)
        {
            int.TryParse(Bind.text, out int value);
            value += num;
            Bind.text = value.ToString();
        }

        public void Subtract(int num)
        {
            int.TryParse(Bind.text, out int value);
            value -= num;
            Bind.text = value.ToString();
        }

        public void Add(float num)
        {
            float.TryParse(Bind.text, out float value);
            value += num;
            Bind.text = value.ToString();
        }

        public void Subtract(float num)
        {
            float.TryParse(Bind.text, out float value);
            value -= num;
            Bind.text = value.ToString();
        }
    }
}
