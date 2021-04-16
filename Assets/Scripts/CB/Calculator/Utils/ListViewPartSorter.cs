using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view sorter for parts
    /// </summary>
    public class ListViewPartSorter : IComparer<string>
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
        public ListViewUtils.SortType JointType = ListViewUtils.SortType.None;
        public ListViewUtils.SortType Size = ListViewUtils.SortType.None;
        public ListViewUtils.SortType MaxLevel = ListViewUtils.SortType.None;
        public ListViewUtils.SortType ExTune = ListViewUtils.SortType.None;
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

            if(Calculator.instance.Parts.ContainsKey(x) && Calculator.instance.Parts.ContainsKey(y))
            {
                if (result == 0)
                {
                    if (COST == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.COST.CompareTo(Calculator.instance.Parts[x].TotalStats.COST);
                    else if (COST == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.COST.CompareTo(Calculator.instance.Parts[y].TotalStats.COST);
                }

                if (result == 0)
                {
                    if (CAPA == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.CAPA.CompareTo(Calculator.instance.Parts[x].TotalStats.CAPA);
                    else if (CAPA == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.CAPA.CompareTo(Calculator.instance.Parts[y].TotalStats.CAPA);
                }

                if (result == 0)
                {
                    if (HP == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.HP.CompareTo(Calculator.instance.Parts[x].TotalStats.HP);
                    else if (HP == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.HP.CompareTo(Calculator.instance.Parts[y].TotalStats.HP);
                }

                if (result == 0)
                {
                    if (STR == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.STR.CompareTo(Calculator.instance.Parts[x].TotalStats.STR);
                    else if (STR == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.STR.CompareTo(Calculator.instance.Parts[y].TotalStats.STR);
                }

                if (result == 0)
                {
                    if (TEC == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.TEC.CompareTo(Calculator.instance.Parts[x].TotalStats.TEC);
                    else if (TEC == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.TEC.CompareTo(Calculator.instance.Parts[y].TotalStats.TEC);
                }

                if (result == 0)
                {
                    if (WLK == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.WLK.CompareTo(Calculator.instance.Parts[x].TotalStats.WLK);
                    else if (WLK == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.WLK.CompareTo(Calculator.instance.Parts[y].TotalStats.WLK);
                }

                if (result == 0)
                {
                    if (FLY == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.FLY.CompareTo(Calculator.instance.Parts[x].TotalStats.FLY);
                    else if (FLY == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.FLY.CompareTo(Calculator.instance.Parts[y].TotalStats.FLY);
                }

                if (result == 0)
                {
                    if (TGH == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.TGH.CompareTo(Calculator.instance.Parts[x].TotalStats.TGH);
                    else if (TGH == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.TGH.CompareTo(Calculator.instance.Parts[y].TotalStats.TGH);
                }

                if (result == 0)
                {
                    if (JointType == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].Root.Joint.CompareTo(Calculator.instance.Parts[x].Root.Joint);
                    else if (JointType == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].Root.Joint.CompareTo(Calculator.instance.Parts[y].Root.Joint);
                }

                if (result == 0)
                {
                    if (Calculator.instance.Parts[x].Root.EquipedPart != null && Calculator.instance.Parts[y].Root.EquipedPart != null)
                    {
                        if (Size == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].Root.EquipedPart.Size.CompareTo(Calculator.instance.Parts[x].Root.EquipedPart.Size);
                        else if (Size == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].Root.EquipedPart.Size.CompareTo(Calculator.instance.Parts[y].Root.EquipedPart.Size);
                    }
                }

                if (result == 0)
                {
                    if (MaxLevel == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].MaxLevel.CompareTo(Calculator.instance.Parts[x].MaxLevel);
                    else if (MaxLevel == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].MaxLevel.CompareTo(Calculator.instance.Parts[y].MaxLevel);
                }

                if (result == 0)
                {
                    if (ExTune == ListViewUtils.SortType.Descending) result = Calculator.instance.Parts[y].ExTuneCount.CompareTo(Calculator.instance.Parts[x].ExTuneCount);
                    else if (ExTune == ListViewUtils.SortType.Ascending) result = Calculator.instance.Parts[x].ExTuneCount.CompareTo(Calculator.instance.Parts[y].ExTuneCount);
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
