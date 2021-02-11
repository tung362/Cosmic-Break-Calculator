using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using CB.Utils;

namespace CB.UI
{
    /// <summary>
    /// Custom dropdown UI for Color type
    /// </summary>
    public class UIColorDropdown : MonoBehaviour
    {
        public Image Labal;
        public RectTransform DropdownTemplate;
        public RectTransform ContentTemplate;
        public RectTransform ItemTemplate;
        public RectTransform AddButton;
        public List<Color> Options = new List<Color>();

        /*Events*/
        public ColorEvent OnValueChanged;

        /*Cache*/
        private Color _Value = new Color();
        public Color Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                UpdateLabel();
                OnValueChanged.Invoke(Value);
            }
        }
        private float PreviousDropdownHeight;
        private float PreviousContentHeight;
        private List<RectTransform> Items = new List<RectTransform>();

        void Start()
        {
            PreviousDropdownHeight = DropdownTemplate.sizeDelta.y;
            PreviousContentHeight = ContentTemplate.sizeDelta.y;
        }

        #region Dropdown
        public void ToggleOptionMenu()
        {
            if (DropdownTemplate.gameObject.activeSelf) Hide();
            else Show();
        }

        public void Show()
        {
            //Create content
            CreateItems();
            //Enable dropdown menu
            DropdownTemplate.gameObject.SetActive(true);
        }

        public void Hide()
        {
            //Destroy items
            for (int i = 0; i < Items.Count; i++) Destroy(Items[i].gameObject);
            Items.Clear();
            //Reset Content window
            DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, PreviousDropdownHeight);
            ContentTemplate.sizeDelta = new Vector2(ContentTemplate.sizeDelta.x, PreviousContentHeight);
            //Disable dropdown menu
            DropdownTemplate.gameObject.SetActive(false);
        }

        void CreateItems()
        {
            //Resize window to fit all items
            float height = (ItemTemplate.sizeDelta.y * (Options.Count + 1)) + (ContentTemplate.sizeDelta.y - ItemTemplate.sizeDelta.y);
            if (height < PreviousDropdownHeight) DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, height);
            else DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, PreviousDropdownHeight);

            AddButton.anchoredPosition = new Vector2(AddButton.anchoredPosition.x, ItemTemplate.anchoredPosition.y - (ItemTemplate.sizeDelta.y * Options.Count));
            RecalculateContent();

            //Create Items
            for (int i = 0; i < Options.Count; i++)
            {
                RectTransform item = Instantiate(ItemTemplate, ContentTemplate);
                Toggle itemToggle = item.GetComponent<Toggle>();

                //Load any changes
                if (Value == Options[i]) itemToggle.SetIsOnWithoutNotify(true);

                item.gameObject.SetActive(true);
                item.anchoredPosition = new Vector2(item.anchoredPosition.x, ItemTemplate.anchoredPosition.y - (ItemTemplate.sizeDelta.y * i));
                item.Find("Item Label").GetComponentInChildren<Image>().color = Options[i];
                itemToggle.onValueChanged.AddListener(x => OnSelectItem(itemToggle));
                Items.Add(item);
            }
        }

        void OnSelectItem(Toggle toggleTransform)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].transform == toggleTransform.transform)
                {
                    SetValue(Options[i]);
                    Hide();
                    break;
                }
            }
        }

        public void SetValue(Color color)
        {
            _Value = color;
            UpdateLabel();
            OnValueChanged.Invoke(Value);
        }

        public void SetValueNoCallback(Color color)
        {
            _Value = color;
            UpdateLabel();
        }

        public void SetLastOption(Color color)
        {
            if (Options.Count != 0) Options[Options.Count - 1] = color;
        }

        void UpdateLabel()
        {
            Labal.color = _Value;
        }

        void RecalculateContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(ContentTemplate.transform, AddButton);
            ContentTemplate.sizeDelta = new Vector2(ContentTemplate.sizeDelta.x, Mathf.Abs(combinedBounds.min.y));
        }
        #endregion

        #region Slot
        public void AddItem()
        {
            RectTransform slot = Instantiate(ItemTemplate, ContentTemplate);
            slot.gameObject.SetActive(true);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(slot.anchoredPosition.x, ItemTemplate.anchoredPosition.y - (ItemTemplate.sizeDelta.y * Options.Count));
            Options.Add(Color.black);
            Items.Add(slot);

            DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, DropdownTemplate.sizeDelta.y + ItemTemplate.sizeDelta.y);
            AddButton.anchoredPosition = new Vector2(AddButton.anchoredPosition.x, ItemTemplate.anchoredPosition.y + (ItemTemplate.sizeDelta.y * Options.Count));
            RecalculateContent();
            SetValue(Color.black);
        }

        public void RemoveItem(RectTransform item)
        {
            int index = Items.IndexOf(item);
            Options.RemoveAt(index);
            Items.RemoveAt(index);
            Destroy(item.gameObject);

            for(int i = index; i < Items.Count; i++) Items[i].anchoredPosition = new Vector2(Items[i].anchoredPosition.x, Items[i].anchoredPosition.y + ItemTemplate.sizeDelta.y);

            DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, DropdownTemplate.sizeDelta.y - ItemTemplate.sizeDelta.y);
            AddButton.anchoredPosition = new Vector2(AddButton.anchoredPosition.x, ItemTemplate.anchoredPosition.y - (ItemTemplate.sizeDelta.y * Options.Count));
            RecalculateContent();
        }
        #endregion
    }
}
