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
        public ListView.SortType Name = ListView.SortType.None;
        public ListView.SortType COST = ListView.SortType.None;
        public ListView.SortType CAPA = ListView.SortType.None;
        public ListView.SortType HP = ListView.SortType.None;
        public ListView.SortType STR = ListView.SortType.None;
        public ListView.SortType TEC = ListView.SortType.None;
        public ListView.SortType WLK = ListView.SortType.None;
        public ListView.SortType FLY = ListView.SortType.None;
        public ListView.SortType TGH = ListView.SortType.None;
        public ListView.SortType JointType = ListView.SortType.None;
        public ListView.SortType Size = ListView.SortType.None;
        public ListView.SortType MaxLevel = ListView.SortType.None;
        public ListView.SortType ExTune = ListView.SortType.None;
        public ListView.SortType Location = ListView.SortType.None;
        public ListView.SortType Date = ListView.SortType.None;


        public int Compare(string x, string y)
        {
            int result = 0;
            if (result == 0)
            {
                if (Name == ListView.SortType.Descending) result = Path.GetFileNameWithoutExtension(y).CompareTo(Path.GetFileNameWithoutExtension(x));
                else if (Name == ListView.SortType.Ascending) result = Path.GetFileNameWithoutExtension(x).CompareTo(Path.GetFileNameWithoutExtension(y));
            }

            if(Calculator.instance.Parts.ContainsKey(x) && Calculator.instance.Parts.ContainsKey(y))
            {
                if (result == 0)
                {
                    if (COST == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.COST.CompareTo(Calculator.instance.Parts[x].TotalStats.COST);
                    else if (COST == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.COST.CompareTo(Calculator.instance.Parts[y].TotalStats.COST);
                }

                if (result == 0)
                {
                    if (CAPA == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.CAPA.CompareTo(Calculator.instance.Parts[x].TotalStats.CAPA);
                    else if (CAPA == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.CAPA.CompareTo(Calculator.instance.Parts[y].TotalStats.CAPA);
                }

                if (result == 0)
                {
                    if (HP == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.HP.CompareTo(Calculator.instance.Parts[x].TotalStats.HP);
                    else if (HP == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.HP.CompareTo(Calculator.instance.Parts[y].TotalStats.HP);
                }

                if (result == 0)
                {
                    if (STR == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.STR.CompareTo(Calculator.instance.Parts[x].TotalStats.STR);
                    else if (STR == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.STR.CompareTo(Calculator.instance.Parts[y].TotalStats.STR);
                }

                if (result == 0)
                {
                    if (TEC == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.TEC.CompareTo(Calculator.instance.Parts[x].TotalStats.TEC);
                    else if (TEC == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.TEC.CompareTo(Calculator.instance.Parts[y].TotalStats.TEC);
                }

                if (result == 0)
                {
                    if (WLK == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.WLK.CompareTo(Calculator.instance.Parts[x].TotalStats.WLK);
                    else if (WLK == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.WLK.CompareTo(Calculator.instance.Parts[y].TotalStats.WLK);
                }

                if (result == 0)
                {
                    if (FLY == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.FLY.CompareTo(Calculator.instance.Parts[x].TotalStats.FLY);
                    else if (FLY == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.FLY.CompareTo(Calculator.instance.Parts[y].TotalStats.FLY);
                }

                if (result == 0)
                {
                    if (TGH == ListView.SortType.Descending) result = Calculator.instance.Parts[y].TotalStats.TGH.CompareTo(Calculator.instance.Parts[x].TotalStats.TGH);
                    else if (TGH == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].TotalStats.TGH.CompareTo(Calculator.instance.Parts[y].TotalStats.TGH);
                }

                if (result == 0)
                {
                    if (JointType == ListView.SortType.Descending) result = Calculator.instance.Parts[y].Root.Joint.CompareTo(Calculator.instance.Parts[x].Root.Joint);
                    else if (JointType == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].Root.Joint.CompareTo(Calculator.instance.Parts[y].Root.Joint);
                }

                if (result == 0)
                {
                    if (Calculator.instance.Parts[x].Root.EquipedPart != null && Calculator.instance.Parts[y].Root.EquipedPart != null)
                    {
                        if (Size == ListView.SortType.Descending) result = Calculator.instance.Parts[y].Root.EquipedPart.Size.CompareTo(Calculator.instance.Parts[x].Root.EquipedPart.Size);
                        else if (Size == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].Root.EquipedPart.Size.CompareTo(Calculator.instance.Parts[y].Root.EquipedPart.Size);
                    }
                }

                if (result == 0)
                {
                    if (MaxLevel == ListView.SortType.Descending) result = Calculator.instance.Parts[y].MaxLevel.CompareTo(Calculator.instance.Parts[x].MaxLevel);
                    else if (MaxLevel == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].MaxLevel.CompareTo(Calculator.instance.Parts[y].MaxLevel);
                }

                if (result == 0)
                {
                    if (ExTune == ListView.SortType.Descending) result = Calculator.instance.Parts[y].ExTuneCount.CompareTo(Calculator.instance.Parts[x].ExTuneCount);
                    else if (ExTune == ListView.SortType.Ascending) result = Calculator.instance.Parts[x].ExTuneCount.CompareTo(Calculator.instance.Parts[y].ExTuneCount);
                }

                if (result == 0)
                {
                    if (Location == ListView.SortType.Descending) result = y.CompareTo(x);
                    else if (Location == ListView.SortType.Ascending) result = x.CompareTo(y);
                }

                if (result == 0)
                {
                    if (Date == ListView.SortType.Descending) result = File.GetLastWriteTime(y).CompareTo(File.GetLastWriteTime(x));
                    else if (Date == ListView.SortType.Ascending) result = File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y));
                }
            }

            if (result == 0) result = x.CompareTo(y);
            return result;
        }
    }
}
