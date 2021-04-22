using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view sorter for tunes
    /// </summary>
    public class ListViewTuneSorter : IComparer<string>
    {
        public ListViewUtils.SortType Name = ListViewUtils.SortType.None;
        public ListViewUtils.SortType COST = ListViewUtils.SortType.None;
        public ListViewUtils.SortType CAPA = ListViewUtils.SortType.None;
        public ListViewUtils.SortType HP = ListViewUtils.SortType.None;
        public ListViewUtils.SortType STR = ListViewUtils.SortType.None;
        public ListViewUtils.SortType TEC = ListViewUtils.SortType.None;
        public ListViewUtils.SortType WLK = ListViewUtils.SortType.None;
        public ListViewUtils.SortType FLY = ListViewUtils.SortType.None;
        public ListViewUtils.SortType TGH = ListViewUtils.SortType.None;
        public ListViewUtils.SortType Ex = ListViewUtils.SortType.None;
        public ListViewUtils.SortType SuccessRate = ListViewUtils.SortType.None;
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

            if (Calculator.instance.Tunes.ContainsKey(x) && Calculator.instance.Tunes.ContainsKey(y))
            {
                if (result == 0)
                {
                    if (COST == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.COST.CompareTo(Calculator.instance.Tunes[x].Stats.COST);
                    else if (COST == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.COST.CompareTo(Calculator.instance.Tunes[y].Stats.COST);
                }

                if (result == 0)
                {
                    if (CAPA == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.CAPA.CompareTo(Calculator.instance.Tunes[x].Stats.CAPA);
                    else if (CAPA == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.CAPA.CompareTo(Calculator.instance.Tunes[y].Stats.CAPA);
                }

                if (result == 0)
                {
                    if (HP == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.HP.CompareTo(Calculator.instance.Tunes[x].Stats.HP);
                    else if (HP == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.HP.CompareTo(Calculator.instance.Tunes[y].Stats.HP);
                }

                if (result == 0)
                {
                    if (STR == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.STR.CompareTo(Calculator.instance.Tunes[x].Stats.STR);
                    else if (STR == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.STR.CompareTo(Calculator.instance.Tunes[y].Stats.STR);
                }

                if (result == 0)
                {
                    if (TEC == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.TEC.CompareTo(Calculator.instance.Tunes[x].Stats.TEC);
                    else if (TEC == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.TEC.CompareTo(Calculator.instance.Tunes[y].Stats.TEC);
                }

                if (result == 0)
                {
                    if (WLK == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.WLK.CompareTo(Calculator.instance.Tunes[x].Stats.WLK);
                    else if (WLK == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.WLK.CompareTo(Calculator.instance.Tunes[y].Stats.WLK);
                }

                if (result == 0)
                {
                    if (FLY == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.FLY.CompareTo(Calculator.instance.Tunes[x].Stats.FLY);
                    else if (FLY == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.FLY.CompareTo(Calculator.instance.Tunes[y].Stats.FLY);
                }

                if (result == 0)
                {
                    if (TGH == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].Stats.TGH.CompareTo(Calculator.instance.Tunes[x].Stats.TGH);
                    else if (TGH == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].Stats.TGH.CompareTo(Calculator.instance.Tunes[y].Stats.TGH);
                }

                if (result == 0)
                {
                    if (Ex == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].IsEx.CompareTo(Calculator.instance.Tunes[x].IsEx);
                    else if (Ex == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].IsEx.CompareTo(Calculator.instance.Tunes[y].IsEx);
                }

                if (result == 0)
                {
                    if (SuccessRate == ListViewUtils.SortType.Descending) result = Calculator.instance.Tunes[y].SuccessRate.CompareTo(Calculator.instance.Tunes[x].SuccessRate);
                    else if (SuccessRate == ListViewUtils.SortType.Ascending) result = Calculator.instance.Tunes[x].SuccessRate.CompareTo(Calculator.instance.Tunes[y].SuccessRate);
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
            }

            if (result == 0) result = x.CompareTo(y);
            return result;
        }
    }
}
