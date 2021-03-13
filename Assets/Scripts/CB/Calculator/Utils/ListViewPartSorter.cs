using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view sorter for parts
    /// </summary>
    public class ListViewPartSorter : IComparer<string>
    {
        public enum FlagTypes { Name, COST, CAPA, HP, STR, TEC, WLK, FLY, TGH, Type, Size, MaxLevel, ExTune, Location, Date };
        public Bitmask FlagMask = new Bitmask();

        public bool SortByHP = false;

        public int Compare(string x, string y)
        {
            //int result = !SortByHP ? Calculator.instance.Parts[x].TotalStats.COST.CompareTo(Calculator.instance.Parts[y].TotalStats.COST) : Calculator.instance.Parts[x].TotalStats.HP.CompareTo(Calculator.instance.Parts[y].TotalStats.HP);
            int result = 0;
            if (result == 0)
            {
                if (!SortByHP) result = Calculator.instance.Parts[x].TotalStats.COST.CompareTo(Calculator.instance.Parts[y].TotalStats.COST);
                else result = Calculator.instance.Parts[x].TotalStats.HP.CompareTo(Calculator.instance.Parts[y].TotalStats.HP);
            }
            if (result == 0) result = x.CompareTo(y);
            return result;
        }
    }
}
