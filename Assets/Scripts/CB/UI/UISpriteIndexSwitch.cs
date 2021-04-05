using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.UI
{
    /// <summary>
    /// UI effects for switching Image sprites based on index
    /// </summary>
    public class UISpriteIndexSwitch : MonoBehaviour
    {
        public Image Bind;
        public List<Sprite> Sprites;

        public void SwitchImage(int num)
        {
            if (num < Sprites.Count) Bind.sprite = Sprites[num];
        }
    }
}
