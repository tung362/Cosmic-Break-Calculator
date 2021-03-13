using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;
using CB.Calculator.Utils;
using System;

namespace CB.Calculator
{
    /// <summary>
    /// Custom list view UI for parts
    /// </summary>
    public class PartListView : MonoBehaviour
    {
        public RectTransform HeaderContent;
        public RectTransform HeaderContainer;
        public RectTransform ItemContent;
        public RectTransform ItemContainer;
        public List<ListView.ItemField> ItemTemplate = new List<ListView.ItemField>();
        public float Offset = 31.0f;
        public float ScrollBarOffset = 7.0f;

        /*Cache*/
        public ListViewPartSorter Sorter = new ListViewPartSorter();
        public Dictionary<string, List<RectTransform>> Items = new Dictionary<string, List<RectTransform>>();
        public HashSet<string> FilteredItems = new HashSet<string>();
        public HashSet<string> Selections = new HashSet<string>();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Sorter.SortByHP = false;

                List<string> temp = Items.Keys.ToList();
                temp.Sort(Sorter);
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = 0; j < Items[temp[i]].Count; j++)
                    {
                        Items[temp[i]][j].SetSiblingIndex(i);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Sorter.SortByHP = true;

                List<string> temp = Items.Keys.ToList();
                temp.Sort(Sorter);
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = 0; j < Items[temp[i]].Count; j++)
                    {
                        Items[temp[i]][j].SetSiblingIndex(i);
                    }
                }
            }
        }

        void OnDestroy()
        {
            UnsetListeners();
        }

        #region Listeners
        void OnPartsChange(string path, bool changed)
        {
            if(changed) AddItem(path);
            else RemoveItem(path);
            UpdateItemContent();
        }

        void Select(bool toggle, string path)
        {
            if (toggle)
            {
                if(!Selections.Contains(path)) Selections.Add(path);
            }
            else
            {
                if (Selections.Contains(path)) Selections.Remove(path);
            }
        }

        void PreviewItem(string path)
        {
            if(Calculator.instance.Parts.ContainsKey(path))
            {
                //Calculator.instance.Parts[path]
            }
        }
        #endregion

        #region Creation And Removal
        public void AddItem(string path)
        {
            if (!Items.ContainsKey(path) && Calculator.instance.Parts.ContainsKey(path))
            {
                List<RectTransform> item = new List<RectTransform>();
                for (int i = 0; i < ItemTemplate.Count; i++)
                {
                    RectTransform itemField = Instantiate(ItemTemplate[i].Template, ItemTemplate[i].Mask);
                    itemField.gameObject.SetActive(true);
                    item.Add(itemField);
                }
                Toggle selectToggle = item[0].GetComponent<Toggle>();
                selectToggle.onValueChanged.AddListener(x => Select(x, path));
                item[1].GetComponent<Button>().onClick.AddListener(() => PreviewItem(path));
                item[2].GetComponent<TMP_Text>().text = Path.GetFileNameWithoutExtension(path);
                item[3].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.COST.ToString();
                item[4].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.CAPA.ToString();
                item[5].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.HP.ToString();
                item[6].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.STR.ToString();
                item[7].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.TEC.ToString();
                item[8].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.WLK.ToString();
                item[9].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.FLY.ToString();
                item[10].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].TotalStats.TGH.ToString();
                item[11].GetComponent<TMP_Text>().text = "-";
                item[12].GetComponent<TMP_Text>().text = "-";
                item[13].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].MaxLevel.ToString();
                item[14].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].ExTuneCount.ToString();
                item[15].GetComponent<TMP_Text>().text = path;
                item[16].GetComponent<TMP_Text>().text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");

                if(Calculator.instance.Parts[path].Root.EquipedPart != null)
                {
                    item[11].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].Root.Joint.ToString();
                    item[12].GetComponent<TMP_Text>().text = Calculator.instance.Parts[path].Root.EquipedPart.Size.ToString();
                }
                Items.Add(path, item);
            }
        }

        public void RemoveItem(string path)
        {
            if (Items.ContainsKey(path))
            {
                for (int i = 0; i < Items[path].Count; i++) Destroy(Items[path][i].gameObject);
                Items.Remove(path);
            }
        }

        public void SelectAll(bool toggle)
        {
            foreach (KeyValuePair<string, List<RectTransform>> item in Items)
            {
                item.Value[0].GetComponent<Toggle>().isOn = toggle;
            }
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

        }

        public void UpdateHeaderContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(HeaderContent.transform, HeaderContainer);
            HeaderContent.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x), HeaderContent.sizeDelta.y);
        }

        public void UpdateItemContent()
        {
            ItemContent.sizeDelta = new Vector2(ItemContent.sizeDelta.x, (Offset * Items.Count) + ScrollBarOffset);
        }
        #endregion

        #region Utils
        public void SetListeners()
        {
            Calculator.instance.OnPartsChange += OnPartsChange;
        }

        public void UnsetListeners()
        {
            Calculator.instance.OnPartsChange -= OnPartsChange;
        }
        #endregion
    }
}
