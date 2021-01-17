using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.UI
{
    /// <summary>
    /// Opens link when called on
    /// </summary>
    public class UILink : MonoBehaviour
    {
        public void OpenLink(string URL)
        {
            Application.OpenURL(URL);
        }
    }
}
