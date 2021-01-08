using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CB.Utils;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI video url editor for the youtube player
    /// </summary>
    public class VideoUrlEdit : MonoBehaviour
    {
        public RectTransform Content;
        public RectTransform Template;
        public RectTransform AddButton;
        public VideoUrlSlot URLTemplate;
        public Vector2 Offset = new Vector2(0, -30.0f);

        /*Cache*/
        private bool IgnoreSave = false;
        private List<VideoUrlSlot> UrlSlots = new List<VideoUrlSlot>();

        void Start()
        {
            URLTemplate.gameObject.SetActive(false);

            Load();
            if (UrlSlots.Count == 0) AddSlot(false);
        }

        public void AddSlot(bool ShowRemoveButton)
        {
            VideoUrlSlot slot = Instantiate(URLTemplate, Template.transform);
            slot.gameObject.SetActive(true);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(Offset.x, Offset.y * UrlSlots.Count);
            if(!ShowRemoveButton) slot.RemoveButton.gameObject.SetActive(false);
            UrlSlots.Add(slot);

            AddButton.anchoredPosition = new Vector2(Offset.x, Offset.y * UrlSlots.Count);
            RecalculateContent();

            //Save
            Save();
        }

        public void RemoveSlot(VideoUrlSlot slot)
        {
            int index = UrlSlots.IndexOf(slot);
            if(index < 0)
            {
                Debug.Log("Warning! Slot is not in the list, this should not happen! \"RemoveSlot()\" @VideoUrlEdit");
                return;
            }

            for (int i = index + 1; i < UrlSlots.Count; i++)
            {
                RectTransform slotTransform = UrlSlots[i].GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(slotTransform.anchoredPosition.x, slotTransform.anchoredPosition.y - Offset.y);
            }
            UrlSlots.Remove(slot);
            Destroy(slot.gameObject);

            AddButton.anchoredPosition = new Vector2(Offset.x, Offset.y * UrlSlots.Count);
            RecalculateContent();

            //Save
            Save();
        }

        public void Clear()
        {
            for(int i = 0; i < UrlSlots.Count; i++) Destroy(UrlSlots[i].gameObject);
            UrlSlots.Clear();
        }

        public void Save()
        {
            if (IgnoreSave) return;

            VideoUrlLibrary entry = new VideoUrlLibrary();
            for (int i = 0; i < UrlSlots.Count; i++) entry.Urls.Add(UrlSlots[i].UrlInputField.text);
            Calculator.instance.UrlLibrary = entry;
            Calculator.instance.SaveUrlLibrary();
        }

        public void Load()
        {
            IgnoreSave = true;
            Clear();
            for (int i = 0; i < Calculator.instance.UrlLibrary.Urls.Count; i++)
            {
                AddSlot(i != 0);
                UrlSlots[i].UrlInputField.text = Calculator.instance.UrlLibrary.Urls[i];
            }
            IgnoreSave = false;
        }

        public void RecalculateContent()
        {
            Vector3[] startCorners = new Vector3[4];
            Vector3[] endCorners = new Vector3[4];

            Content.GetWorldCorners(startCorners);
            AddButton.GetWorldCorners(endCorners);

            float difference = startCorners[0].y - endCorners[0].y;
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, Content.sizeDelta.y + difference);
        }
    }
}
