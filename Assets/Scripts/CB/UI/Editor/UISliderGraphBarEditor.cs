using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CB.UI
{
    /// <summary>
    /// Custom inspector for UISliderGraphBar
    /// </summary>
    [CustomEditor(typeof(UISliderGraphBar))]
    public class UISliderGraphBarEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UISliderGraphBar targetedScript = (UISliderGraphBar)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Generate")) targetedScript.Generate();
        }
    }
}
