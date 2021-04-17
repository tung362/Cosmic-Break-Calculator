﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using CB.Utils;
using CB.UI;
using CB.Calculator.Utils;
using Cysharp.Threading.Tasks;

namespace CB.Calculator
{
    /// <summary>
    /// Custom list view UI for parts
    /// </summary>
    public class PartListView : MonoBehaviour
    {
        public RectTransform HeaderContent;
        public RectTransform HeaderContainer;
        public RectTransform ItemTemplate;
        public RectTransform ItemContent;
        public RectTransform ItemContainer;
        public List<ListViewUtils.ItemField> PoolTemplate = new List<ListViewUtils.ItemField>();
        public float Offset = 31.0f;
        public float ScrollBarOffset = 7.0f;

        /*Callbacks*/
        public event Action<string> OnPreview;

        /*Cache*/
        private List<IPool<string>> Pool = new List<IPool<string>>();
        private HashSet<string> Items = new HashSet<string>();
        private HashSet<string> Selections = new HashSet<string>();
        private (HashSet<string>, List<string>) SortedItems = (new HashSet<string>(), new List<string>());
        private int TopIndex = 0;
        private string PreviewPath = "";
        private Bitmask JointTypeFilter = new Bitmask();
        private Bitmask BDTypeFilter = new Bitmask();
        private Bitmask SizeTypeFilter = new Bitmask();
        private string SearchText = "";
        private ListViewPartSorter Sorter = new ListViewPartSorter();

        public void Init()
        {
            for (int i = 0; i < (int)PartJoint.JointType.Count; i++) JointTypeFilter.AddFlag(i);
            for (int i = 0; i < (int)Part.BDType.Count; i++) BDTypeFilter.AddFlag(i);
            for (int i = 0; i < (int)Part.SizeType.Count; i++) SizeTypeFilter.AddFlag(i);
            RecalculatePool();
        }

        #region Listeners
        void Select(bool toggle, string path)
        {
            if (toggle)
            {
                if (!Selections.Contains(path)) Selections.Add(path);
            }
            else
            {
                if (Selections.Contains(path)) Selections.Remove(path);
            }
        }

        void PreviewItem(string path)
        {
            //Event callback
            if (Calculator.instance.Parts.ContainsKey(path))
            {
                PreviewPath = path;
                OnPreview?.Invoke(path);
            }
        }
        #endregion

        #region Creation And Removal
        public void OnPartsChange(string path, bool changed)
        {
            if (changed)
            {
                if (PreviewPath == path) OnPreview?.Invoke(path);
                AddItem(path);
            }
            else RemoveItem(path);
        }

        public void OnPartsFinish()
        {
            SortItems();
            UpdateItemContent();
            UpdatePool();
        }

        public void RecalculatePool()
        {
            int poolCount = Mathf.CeilToInt(ItemTemplate.rect.height / Mathf.Abs(Offset)) + 1;
            int difference = (poolCount < 0 ? 0 : poolCount) - Pool.Count;

            if (difference >= 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    List<RectTransform> item = new List<RectTransform>();
                    for (int j = 0; j < PoolTemplate.Count; j++)
                    {
                        RectTransform itemField = Instantiate(PoolTemplate[j].Template, PoolTemplate[j].Mask);
                        itemField.gameObject.SetActive(true);
                        item.Add(itemField);
                    }
                    Pool.Add(new ListViewPartPoolItem(item));
                }
            }
            else
            {
                for (int i = 0; i < -difference; i++)
                {
                    int lastIndex = Pool.Count - 1;
                    Pool[lastIndex].Destroy();
                    Pool.RemoveAt(lastIndex);
                }
            }
            UpdatePool();
        }

        public void UpdateScroll()
        {
            int currentTopIndex = Mathf.FloorToInt(ItemContent.anchoredPosition.y / Offset);
            if (currentTopIndex < 0) currentTopIndex = 0;
            if (currentTopIndex != TopIndex)
            {
                ItemContainer.anchoredPosition = new Vector2(ItemContainer.anchoredPosition.x, -Offset * currentTopIndex);
                TopIndex = currentTopIndex;
                UpdatePool();
            }
        }

        public void UpdatePool()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                ListViewPartPoolItem poolItem = (ListViewPartPoolItem)Pool[i];

                int index = TopIndex + i;
                if (index >= SortedItems.Item2.Count)
                {
                    poolItem.SetActive(false);
                    poolItem.SelectToggle.onValueChanged.RemoveAllListeners();
                    poolItem.BackgroundButton.onClick.RemoveAllListeners();
                }
                else
                {
                    poolItem.SetActive(true);
                    poolItem.SelectToggle.onValueChanged.RemoveAllListeners();
                    poolItem.BackgroundButton.onClick.RemoveAllListeners();
                    poolItem.UpdateItem(SortedItems.Item2[index], Selections.Contains(SortedItems.Item2[index]));
                    poolItem.SelectToggle.onValueChanged.AddListener(x => Select(x, SortedItems.Item2[index]));
                    poolItem.BackgroundButton.onClick.AddListener(() => PreviewItem(SortedItems.Item2[index]));
                }
            }
        }

        public void AddItem(string path)
        {
            if (!Items.Contains(path))
            {
                //Add to list of all items
                Items.Add(path);

                //Filter item
                if (!SortedItems.Item1.Contains(path))
                {
                    if (CheckFilter(path))
                    {
                        SortedItems.Item2.Add(path);
                        SortedItems.Item1.Add(path);
                    }
                }
            }
        }

        public void RemoveItem(string path)
        {
            if (Items.Contains(path))
            {
                if(SortedItems.Item1.Contains(path))
                {
                    SortedItems.Item2.Remove(path);
                    SortedItems.Item1.Remove(path);
                }
                Items.Remove(path);

                if (PreviewPath == path) ResetPreview();
                if (Selections.Contains(path)) Selections.Remove(path);

                UpdateItemContent();
                UpdatePool();
            }
        }

        public void SelectAll(bool toggle)
        {
            for (int i = 0; i < SortedItems.Item2.Count; i++) Select(toggle, SortedItems.Item2[i]);
            UpdatePool();
        }

        public void DeleteSelectedItems()
        {
            foreach (string path in Selections)
            {
                if (File.Exists(path)) File.Delete(path);
            }
            Selections.Clear();
        }

        public void ResetPreview()
        {
            PreviewPath = "";
            //Event callback
            OnPreview?.Invoke(PreviewPath);
        }

        public void UpdateHeaderContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(HeaderContent.transform, HeaderContainer);
            HeaderContent.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x), HeaderContent.sizeDelta.y);
        }

        public void UpdateItemContent()
        {
            ItemContent.sizeDelta = new Vector2(ItemContent.sizeDelta.x, (Offset * SortedItems.Item2.Count) + ScrollBarOffset);
        }
        #endregion

        #region Utils
        bool CheckFilter(string path)
        {
            if (!Calculator.instance.Parts.ContainsKey(path)) return false;
            if (!JointTypeFilter.HasFlag((int)Calculator.instance.Parts[path].Root.Joint)) return false;
            if (Calculator.instance.Parts[path].Root.EquipedPart != null)
            {
                bool match = false;
                for (int i = 0; i < (int)Part.BDType.Count; i++)
                {
                    if (BDTypeFilter.HasFlag(i))
                    {
                        if (Calculator.instance.Parts[path].Root.EquipedPart.BDMask.HasFlag(i))
                        {
                            match = true;
                            break;
                        }
                    }
                }
                if (!match) return false;
                if (!SizeTypeFilter.HasFlag((int)Calculator.instance.Parts[path].Root.EquipedPart.Size)) return false;
            }
            if(!string.IsNullOrEmpty(SearchText))
            {
                if (!Path.GetFileNameWithoutExtension(path).ToLowerInvariant().Contains(SearchText.ToLowerInvariant())) return false;
            }
            return true;
        }

        public void FilterItems(bool toggle)
        {
            if (toggle)
            {
                foreach (string item in Items)
                {
                    if (!SortedItems.Item1.Contains(item))
                    {
                        if (CheckFilter(item))
                        {
                            SortedItems.Item2.Add(item);
                            SortedItems.Item1.Add(item);
                        }
                    }
                }
                SortItems();
            }
            else
            {
                for (int i = 0; i < SortedItems.Item2.Count; i++)
                {
                    if (!CheckFilter(SortedItems.Item2[i]))
                    {
                        if (Selections.Contains(SortedItems.Item2[i])) Selections.Remove(SortedItems.Item2[i]);
                        SortedItems.Item1.Remove(SortedItems.Item2[i]);
                        SortedItems.Item2.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void FilterItems()
        {
            foreach (string item in Items)
            {
                if (!SortedItems.Item1.Contains(item))
                {
                    if (CheckFilter(item))
                    {
                        SortedItems.Item2.Add(item);
                        SortedItems.Item1.Add(item);
                    }
                    SortItems();
                }
                else
                {
                    if (!CheckFilter(item))
                    {
                        if (Selections.Contains(item)) Selections.Remove(item);
                        SortedItems.Item1.Remove(item);
                        SortedItems.Item2.Remove(item);
                    }
                }
            }
        }

        void SortItems()
        {
            SortedItems.Item2.Sort(Sorter);
        }
        #endregion

        #region Filter Editing
        public void FilterBD(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.BD);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.BD);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterLG(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.LG);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.LG);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterHD(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.HD);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.HD);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterHAC(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.HAC);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.HAC);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterFAC(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.FAC);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.FAC);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterAM(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.AM);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.AM);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterBS(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.BS);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.BS);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterWP(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.WP);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.WP);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterWB(bool toggle)
        {
            if (toggle) JointTypeFilter.AddFlag((int)PartJoint.JointType.WB);
            else JointTypeFilter.RemoveFlag((int)PartJoint.JointType.WB);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterLnd(bool toggle)
        {
            if (toggle) BDTypeFilter.AddFlag((int)Part.BDType.Lnd);
            else BDTypeFilter.RemoveFlag((int)Part.BDType.Lnd);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterAir(bool toggle)
        {
            if (toggle) BDTypeFilter.AddFlag((int)Part.BDType.Air);
            else BDTypeFilter.RemoveFlag((int)Part.BDType.Air);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterArt(bool toggle)
        {
            if (toggle) BDTypeFilter.AddFlag((int)Part.BDType.Art);
            else BDTypeFilter.RemoveFlag((int)Part.BDType.Art);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterSup(bool toggle)
        {
            if (toggle) BDTypeFilter.AddFlag((int)Part.BDType.Sup);
            else BDTypeFilter.RemoveFlag((int)Part.BDType.Sup);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterNone(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.None);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.None);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterSS(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.SS);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.SS);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterS(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.S);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.S);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterM(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.M);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.M);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterL(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.L);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.L);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterLL(bool toggle)
        {
            if (toggle) SizeTypeFilter.AddFlag((int)Part.SizeType.LL);
            else SizeTypeFilter.RemoveFlag((int)Part.SizeType.LL);
            FilterItems(toggle);
            UpdateItemContent();
            UpdatePool();
        }

        public void FilterSearch(string text)
        {
            SearchText = text;
            FilterItems();
            UpdateItemContent();
            UpdatePool();
        }
        #endregion

        #region Sort Editing
        public void SortName(int num)
        {
            Sorter.Name = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortCOST(int num)
        {
            Sorter.COST = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortCAPA(int num)
        {
            Sorter.CAPA = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortHP(int num)
        {
            Sorter.HP = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortSTR(int num)
        {
            Sorter.STR = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortTEC(int num)
        {
            Sorter.TEC = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortWLK(int num)
        {
            Sorter.WLK = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortFLY(int num)
        {
            Sorter.FLY = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortTGH(int num)
        {
            Sorter.TGH = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortJointType(int num)
        {
            Sorter.JointType = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortSize(int num)
        {
            Sorter.Size = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortMaxLevel(int num)
        {
            Sorter.MaxLevel = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortExTune(int num)
        {
            Sorter.ExTune = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortLocation(int num)
        {
            Sorter.Location = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }

        public void SortDate(int num)
        {
            Sorter.Date = (ListViewUtils.SortType)num;
            SortItems();
            UpdatePool();
        }
        #endregion
    }
}
