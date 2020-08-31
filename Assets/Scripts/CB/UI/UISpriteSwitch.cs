using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching Image sprites
    /// </summary>
    public class UISpriteSwitch : MonoBehaviour
    {
        public Image Bind;
        public Sprite OnSprite;
        public Sprite OffSprite;

        public void SwitchImage(bool toggle)
        {
            Bind.sprite = toggle ? OnSprite : OffSprite;
        }
    }
}
