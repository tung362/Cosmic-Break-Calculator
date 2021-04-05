using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Utils
{
    /// <summary>
    /// List view utilities
    /// </summary>
    public static class ListView
    {
        #region Format
        [System.Serializable]
        public class ItemField
        {
            public RectTransform Mask;
            public RectTransform Template;
        }

        public enum SortType { None, Descending, Ascending };
        #endregion
    }
}
