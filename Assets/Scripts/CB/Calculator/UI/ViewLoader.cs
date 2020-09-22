using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI loader for joints and parts viewer
    /// </summary>
    public class ViewLoader : MonoBehaviour
    {
        public PartBuilder Builder;

        /*Binds*/

        void OnEnable()
        {
            //Set listener
            Builder.OnRedraw += OnRedraw;
        }

        void OnDisable()
        {
            //Unset listener
            Builder.OnRedraw -= OnRedraw;
        }

        void OnRedraw(ISelectable selectable)
        {
            if (selectable != null)
            {
                //If the selectable type is a PartJointSlot
                if (selectable.GetType() == typeof(PartJointSlot))
                {
                    PartJointSlot jointSlot = (PartJointSlot)selectable;
                }

                //If the selectable type is a TuneSlot
                if (selectable.GetType() == typeof(TuneSlot))
                {
                    TuneSlot tuneSlot = (TuneSlot)selectable;
                }
            }
        }
    }
}
