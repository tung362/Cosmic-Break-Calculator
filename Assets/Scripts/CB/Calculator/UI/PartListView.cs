using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using CB.Utils;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Custom list view UI for Part
    /// </summary>
    public class PartListView : MonoBehaviour
    {
        public RectTransform HeaderContent;
        public RectTransform HeaderContainer;
        public RectTransform ItemContent;
        public RectTransform ItemContainer;
        public List<ListView.ItemField> ItemTemplate = new List<ListView.ItemField>();
        public Vector2 Offset = new Vector2(0.0f, -31.0f);
        public float ScrollBarOffset = 7.0f;

        /*Cache*/
        public Dictionary<string, List<RectTransform>> Items = new Dictionary<string, List<RectTransform>>();
        private List<DirectoryWatcher> Watchers = new List<DirectoryWatcher>();

        void OnDestroy()
        {
            UnsetListeners();
        }

        #region Listeners
        void OnFileChanged(string path, string root)
        {
            if (!Items.ContainsKey(path))
            {
                AddItem(path);
                UpdateItemContent();
            }
        }

        void OnDeleted(string path, string root)
        {
            if (Items.ContainsKey(path))
            {
                RemoveItem(path);
                UpdateItemContent();
            }
        }
        #endregion

        #region Creation And Removal
        public void AddItem(string path)
        {
            if (!Items.ContainsKey(path))
            {
                if (Serializer.Load(path, out Contraption result) && result != null)
                {
                    List<RectTransform> item = new List<RectTransform>();
                    for (int i = 0; i < ItemTemplate.Count; i++)
                    {
                        RectTransform itemField = Instantiate(ItemTemplate[i].Template, ItemTemplate[i].Mask);
                        itemField.gameObject.SetActive(true);
                        itemField.anchoredPosition = new Vector2(ItemTemplate[i].Template.anchoredPosition.x, Offset.y * Items.Count);
                        item.Add(itemField);
                    }
                    item[2].GetComponent<TMP_Text>().text = Path.GetFileNameWithoutExtension(path);
                    item[3].GetComponent<TMP_Text>().text = result.TotalStats.COST.ToString();
                    item[4].GetComponent<TMP_Text>().text = result.TotalStats.CAPA.ToString();
                    item[5].GetComponent<TMP_Text>().text = result.TotalStats.HP.ToString();
                    item[6].GetComponent<TMP_Text>().text = result.TotalStats.STR.ToString();
                    item[7].GetComponent<TMP_Text>().text = result.TotalStats.TEC.ToString();
                    item[8].GetComponent<TMP_Text>().text = result.TotalStats.WLK.ToString();
                    item[9].GetComponent<TMP_Text>().text = result.TotalStats.FLY.ToString();
                    item[10].GetComponent<TMP_Text>().text = result.TotalStats.TGH.ToString();
                    item[11].GetComponent<TMP_Text>().text = result.Root.Joint.ToString();
                    item[12].GetComponent<TMP_Text>().text = result.Root.EquipedPart != null ? result.Root.EquipedPart.Size.ToString() : "-";
                    item[13].GetComponent<TMP_Text>().text = result.MaxLevel.ToString();
                    item[14].GetComponent<TMP_Text>().text = result.ExTuneCount.ToString();
                    item[15].GetComponent<TMP_Text>().text = path;
                    item[16].GetComponent<TMP_Text>().text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
                    Items.Add(path, item);
                }
            }
        }

        public void RemoveItem(string path)
        {
            if (Items.ContainsKey(path))
            {

            }
        }

        public void RemoveSelectedItems()
        {

        }

        public void SelectAll()
        {

        }

        public void UnselectAll()
        {

        }

        public void Select()
        {

        }

        public void Unselect()
        {

        }

        public void UpdateHeaderContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(HeaderContent.transform, HeaderContainer);
            HeaderContent.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x), HeaderContent.sizeDelta.y);
        }

        public void UpdateItemContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(ItemContent.transform, ItemContainer);
            ItemContent.sizeDelta = new Vector2(ItemContent.sizeDelta.x, Mathf.Abs(combinedBounds.min.y) + ScrollBarOffset);
        }
        #endregion

        #region Utils
        public void SetListeners(DirectoryWatcher watcher)
        {
            watcher.OnFileChanged += OnFileChanged;
            watcher.OnDeleted += OnDeleted;
            Watchers.Add(watcher);
        }

        public void UnsetListeners()
        {
            for (int i = 0; i < Watchers.Count; i++)
            {
                Watchers[i].OnFileChanged -= OnFileChanged;
                Watchers[i].OnDeleted -= OnDeleted;
            }
        }
        #endregion
    }
}
