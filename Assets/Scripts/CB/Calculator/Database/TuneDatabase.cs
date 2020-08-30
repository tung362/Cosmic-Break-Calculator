using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Malee.List;

namespace CB.Calculator.Database
{
    /// <summary>
    /// Database of all registered tunes
    /// </summary>
    [CreateAssetMenu(menuName = "CB/Tune Database", fileName = "TuneDatabase.asset")]
    public class TuneDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Format
        [System.Serializable]
        public class TuneList : ReorderableArray<Tune> { }
        #endregion

        [Reorderable]
        public TuneList Tunes = new TuneList();

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
            Tunes = new TuneList();

            //Save
            if (save) Save();
        }
        #endif
    }
}
