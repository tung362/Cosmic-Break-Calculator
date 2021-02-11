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
    /// Custom dropdown UI for Bitmask type
    /// </summary>
    public class UIMaskDropdown : MonoBehaviour
    {
        public TextMeshProUGUI Labal;
        public RectTransform DropdownTemplate;
        public RectTransform ContentTemplate;
        public RectTransform ItemTemplate;
        public List<string> Options = new List<string>();

        /*Events*/
        public BitMaskEvent OnValueChanged;

        /*Cache*/
        private Bitmask _Value = new Bitmask();
        public Bitmask Value
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
            float height = (ItemTemplate.sizeDelta.y * Options.Count) + (ContentTemplate.sizeDelta.y - ItemTemplate.sizeDelta.y);
            if (height < PreviousDropdownHeight) DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, height);
            else DropdownTemplate.sizeDelta = new Vector2(DropdownTemplate.sizeDelta.x, PreviousDropdownHeight);
            ContentTemplate.sizeDelta = new Vector2(ContentTemplate.sizeDelta.x, height);

            //Create Items
            for (int i = 0; i < Options.Count; i++)
            {
                RectTransform item = Instantiate(ItemTemplate, ContentTemplate);
                Toggle itemToggle = item.GetComponent<Toggle>();

                //Load any changes
                if (Value.HasFlag(i)) itemToggle.SetIsOnWithoutNotify(true);

                item.gameObject.SetActive(true);
                item.anchoredPosition = new Vector2(item.anchoredPosition.x, ItemTemplate.anchoredPosition.y + (item.sizeDelta.y * ((Options.Count - 1) - i)));
                item.GetComponentInChildren<TextMeshProUGUI>().text = Options[i];
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
                    if (toggleTransform.isOn) AddFlag(i);
                    else RemoveFlag(i);
                    break;
                }
            }
        }

        public void UpdateLabel()
        {
            if (Value.Mask == 0)
            {
                Labal.text = "Nothing";
                return;
            }

            int activeFlagCount = 0;
            for(int i = 0; i < Options.Count; i++)
            {
                if (Value.HasFlag(i))
                {
                    Labal.text = Options[i];
                    activeFlagCount++;
                }
            }

            if(activeFlagCount >= 2)
            {
                if (activeFlagCount == Options.Count)
                {
                    Labal.text = "Everything";
                    return;
                }
                Labal.text = "Mixed";
            }
        }

        public void SetValueNoCallback(Bitmask mask)
        {
            _Value = mask;
            UpdateLabel();
        }

        public void AddFlag(int flag)
        {
            _Value.AddFlag(flag);
            UpdateLabel();
            OnValueChanged.Invoke(Value);
        }

        public void AddFlagNoCallback(int flag)
        {
            _Value.AddFlag(flag);
            UpdateLabel();
        }

        public void RemoveFlag(int flag)
        {
            _Value.RemoveFlag(flag);
            UpdateLabel();
            OnValueChanged.Invoke(Value);
        }

        public void RemoveFlagNoCallback(int flag)
        {
            _Value.RemoveFlag(flag);
            UpdateLabel();
        }
    }
}
