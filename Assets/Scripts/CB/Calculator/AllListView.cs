using System.Collections;
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
    /// Custom list view UI for builds, parts, tunes, and cartridges
    /// </summary>
    public class AllListView : MonoBehaviour
    {
        /*Enums*/
        public RectTransform HeaderContent;
        public RectTransform HeaderContainer;
        public RectTransform ItemTemplate;
        public RectTransform ItemContent;
        public RectTransform ItemContainer;
        public List<ListViewUtils.ItemField> PoolTemplate = new List<ListViewUtils.ItemField>();
        public float Offset = 31.0f;
        public float ScrollBarOffset = 7.0f;

        /*Callbacks*/
        public event Action<string, Calculator.SaveType> OnPreview;

        /*Cache*/
        private List<IPool<string>> Pool = new List<IPool<string>>();
        private Dictionary<string, Calculator.SaveType> Items = new Dictionary<string, Calculator.SaveType>();
        private HashSet<string> Selections = new HashSet<string>();
        private (HashSet<string>, List<string>) SortedItems = (new HashSet<string>(), new List<string>());
        private int TopIndex = 0;
        private string PreviewPath = "";
        private string SearchText = "";
        private ListViewAllSorter Sorter = new ListViewAllSorter();

        public void Init()
        {
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

        void PreviewItem(string path, Calculator.SaveType saveType)
        {
            //Event callback
            PreviewPath = path;
            OnPreview?.Invoke(path, saveType);
        }
        #endregion

        #region Creation And Removal
        public void OnBuildsChange(string path, bool changed)
        {
            if (changed)
            {
                if (PreviewPath == path) OnPreview?.Invoke(path, Calculator.SaveType.Build);
                AddItem(path, Calculator.SaveType.Build);
            }
            else RemoveItem(path);
        }

        public void OnPartsChange(string path, bool changed)
        {
            if (changed)
            {
                if (PreviewPath == path) OnPreview?.Invoke(path, Calculator.SaveType.Part);
                AddItem(path, Calculator.SaveType.Part);
            }
            else RemoveItem(path);
        }

        public void OnTunesChange(string path, bool changed)
        {
            if (changed)
            {
                if (PreviewPath == path) OnPreview?.Invoke(path, Calculator.SaveType.Tune);
                AddItem(path, Calculator.SaveType.Tune);
            }
            else RemoveItem(path);
        }
        public void OnCartridgesChange(string path, bool changed)
        {
            if (changed)
            {
                if (PreviewPath == path) OnPreview?.Invoke(path, Calculator.SaveType.Cartridge);
                AddItem(path, Calculator.SaveType.Cartridge);
            }
            else RemoveItem(path);
        }

        public void OnFinish()
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
                    Pool.Add(new ListViewAllPoolItem(item));
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
                ListViewAllPoolItem poolItem = (ListViewAllPoolItem)Pool[i];

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
                    poolItem.BackgroundButton.onClick.AddListener(() => PreviewItem(SortedItems.Item2[index], Items[SortedItems.Item2[index]]));
                }
            }
        }

        public void AddItem(string path, Calculator.SaveType saveType)
        {
            if (!Items.ContainsKey(path))
            {
                //Add to list of all items
                Items.Add(path, saveType);

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
            if (Items.ContainsKey(path))
            {
                if (SortedItems.Item1.Contains(path))
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
            OnPreview?.Invoke(PreviewPath, Calculator.SaveType.None);
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
            if (Items.ContainsKey(path))
            {
                switch (Items[path])
                {
                    case Calculator.SaveType.Build:
                        if (!Calculator.instance.Builds.ContainsKey(path)) return false;
                        break;
                    case Calculator.SaveType.Part:
                        if (!Calculator.instance.Parts.ContainsKey(path)) return false;
                        break;
                    case Calculator.SaveType.Tune:
                        if (!Calculator.instance.Tunes.ContainsKey(path)) return false;
                        break;
                    case Calculator.SaveType.Cartridge:
                        if (!Calculator.instance.Cartridges.ContainsKey(path)) return false;
                        break;
                    default:
                        return false;
                }
            }
            else return false;
            if (!string.IsNullOrEmpty(SearchText))
            {
                if (!Path.GetFileNameWithoutExtension(path).ToLowerInvariant().Contains(SearchText.ToLowerInvariant())) return false;
            }
            return true;
        }

        public void FilterItems(bool toggle)
        {
            if (toggle)
            {
                foreach (KeyValuePair<string, Calculator.SaveType> item in Items)
                {
                    if (!SortedItems.Item1.Contains(item.Key))
                    {
                        if (CheckFilter(item.Key))
                        {
                            SortedItems.Item2.Add(item.Key);
                            SortedItems.Item1.Add(item.Key);
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
            foreach (KeyValuePair<string, Calculator.SaveType> item in Items)
            {
                if (!SortedItems.Item1.Contains(item.Key))
                {
                    if (CheckFilter(item.Key))
                    {
                        SortedItems.Item2.Add(item.Key);
                        SortedItems.Item1.Add(item.Key);
                    }
                    SortItems();
                }
                else
                {
                    if (!CheckFilter(item.Key))
                    {
                        if (Selections.Contains(item.Key)) Selections.Remove(item.Key);
                        SortedItems.Item1.Remove(item.Key);
                        SortedItems.Item2.Remove(item.Key);
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
