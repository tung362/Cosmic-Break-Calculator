using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view sorter for cartridges
    /// </summary>
    public class ListViewCartridgeSorter : IComparer<string>
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

            if (Calculator.instance.Cartridges.ContainsKey(x) && Calculator.instance.Cartridges.ContainsKey(y))
            {
                if (result == 0)
                {
                    if (COST == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.COST.CompareTo(Calculator.instance.Cartridges[x].Stats.COST);
                    else if (COST == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.COST.CompareTo(Calculator.instance.Cartridges[y].Stats.COST);
                }

                if (result == 0)
                {
                    if (CAPA == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.CAPA.CompareTo(Calculator.instance.Cartridges[x].Stats.CAPA);
                    else if (CAPA == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.CAPA.CompareTo(Calculator.instance.Cartridges[y].Stats.CAPA);
                }

                if (result == 0)
                {
                    if (HP == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.HP.CompareTo(Calculator.instance.Cartridges[x].Stats.HP);
                    else if (HP == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.HP.CompareTo(Calculator.instance.Cartridges[y].Stats.HP);
                }

                if (result == 0)
                {
                    if (STR == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.STR.CompareTo(Calculator.instance.Cartridges[x].Stats.STR);
                    else if (STR == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.STR.CompareTo(Calculator.instance.Cartridges[y].Stats.STR);
                }

                if (result == 0)
                {
                    if (TEC == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.TEC.CompareTo(Calculator.instance.Cartridges[x].Stats.TEC);
                    else if (TEC == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.TEC.CompareTo(Calculator.instance.Cartridges[y].Stats.TEC);
                }

                if (result == 0)
                {
                    if (WLK == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.WLK.CompareTo(Calculator.instance.Cartridges[x].Stats.WLK);
                    else if (WLK == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.WLK.CompareTo(Calculator.instance.Cartridges[y].Stats.WLK);
                }

                if (result == 0)
                {
                    if (FLY == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.FLY.CompareTo(Calculator.instance.Cartridges[x].Stats.FLY);
                    else if (FLY == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.FLY.CompareTo(Calculator.instance.Cartridges[y].Stats.FLY);
                }

                if (result == 0)
                {
                    if (TGH == ListViewUtils.SortType.Descending) result = Calculator.instance.Cartridges[y].Stats.TGH.CompareTo(Calculator.instance.Cartridges[x].Stats.TGH);
                    else if (TGH == ListViewUtils.SortType.Ascending) result = Calculator.instance.Cartridges[x].Stats.TGH.CompareTo(Calculator.instance.Cartridges[y].Stats.TGH);
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
