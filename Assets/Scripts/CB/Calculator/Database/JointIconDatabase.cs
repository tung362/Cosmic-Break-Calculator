using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Malee.List;
using RotaryHeart.Lib.SerializableDictionary;

namespace CB.Calculator.Database
{
    /// <summary>
    /// Database of all registered joint icons
    /// </summary>
    [CreateAssetMenu(menuName = "CB/Joint Icon Database", fileName = "JointIconDatabase.asset")]
    public class JointIconDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Format
        [System.Serializable]
        public class JointIconDictionary : SerializableDictionaryBase<PartJoint.JointType, Sprite> { }
        #endregion

        public JointIconDictionary Icons = new JointIconDictionary();

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }

        #if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void ClearDatabase(bool save = true)
        {
            Icons = new JointIconDictionary();

            //Save
            if (save) Save();
        }
        #endif
    }
}
