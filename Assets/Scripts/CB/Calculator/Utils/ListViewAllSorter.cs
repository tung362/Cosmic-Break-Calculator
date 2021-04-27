using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view sorter for builds, parts, tunes, and cartridges
    /// </summary>
    public class ListViewAllSorter : IComparer<string>
    {
        public ListViewUtils.SortType Name = ListViewUtils.SortType.None;
        public ListViewUtils.SortType Location = ListViewUtils.SortType.None;
        public ListViewUtils.SortType Date = ListViewUtils.SortType.None;


        public int Compare(string x, string y)
        {
            int result = 0;
            if (result == 0)
            {
                if (Name == ListViewUtils.SortType.Descending) result = Path.GetFileNameWithoutExtension(y).CompareTo(Path.GetFileNameWithoutExtension(x));
                else if (Name == ListViewUtils.SortType.Ascending) result = Path.GetFileNameWithoutExtension(x).CompareTo(Path.GetFileNameWithoutExtension(y));
            }

            if (result == 0)
            {
                if (Location == ListViewUtils.SortType.Descending) result = y.CompareTo(x);
                else if (Location == ListViewUtils.SortType.Ascending) result = x.CompareTo(y);
            }

            if (result == 0)
            {
                if (Date == ListViewUtils.SortType.Descending) result = File.GetLastWriteTime(y).CompareTo(File.GetLastWriteTime(x));
                else if (Date == ListViewUtils.SortType.Ascending) result = File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y));
            }

            if (result == 0) result = x.CompareTo(y);
            return result;
        }
    }
}
