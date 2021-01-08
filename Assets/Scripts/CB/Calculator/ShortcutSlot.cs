using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Calculator.UI;

namespace CB.Calculator
{
    /// <summary>
    /// UI representation of a shortcut
    /// </summary>
    public class ShortcutSlot : MonoBehaviour
    {
        public KeyCode Key = KeyCode.None;
        public bool DetectKey = false;

        [Header("Binds")]
        public TMP_Text ShortcutText;
        public TMP_Text CombineText;
        public ShortcutsEdit Builder;

        /*Cache*/

        void Update()
        {
            if(DetectKey)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(key))
                        {
                            SetValue(key);
                            DetectKey = false;
                            break;
                        }
                    }
                }
            }
        }

        public void BindKey()
        {
            DetectKey = true;
        }

        public void SetValue(KeyCode key)
        {
            Key = key;
            ShortcutText.text = key.ToString();
            Builder.Save();
        }
    }
}
