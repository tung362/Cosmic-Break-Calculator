using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CB.UI
{
    /// <summary>
    /// Custom inspector for UIRegionCollapse
    /// </summary>
    [CustomEditor(typeof(UIRegionCollapse))]
    public class UIRegionCollapseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UIRegionCollapse targetedScript = (UIRegionCollapse)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Remove Unused")) targetedScript.RemoveAllEmpty();
        }
    }
}
