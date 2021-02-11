using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI shortcuts editor
    /// </summary>
    public class ShortcutsEdit : MonoBehaviour
    {
        /*Enums*/
        public enum ShortcutType { SettingsMenu, Screenshot, Undo, Redo, Save, SaveAs, Load, NewFile };

        /*Configuration*/
        public RectTransform Template;
        public RectTransform AddButton;
        public Animator ResetButton;
        public ShortcutSlot ShortcutTemplate;
        public int SlotLimit = 4;
        public Vector2 Offset = new Vector2(-124.0f, 0);
        public ShortcutType ShortcutField;

        /*Cache*/
        private bool IgnoreSave = false;
        private List<ShortcutSlot> ShortcutSlots = new List<ShortcutSlot>();

        void Start()
        {
            ShortcutTemplate.gameObject.SetActive(false);

            Load();
            if (ShortcutSlots.Count == 0) AddSlot(false);
        }

        public void AddSlot(bool ShowRemoveButton)
        {
            if (ShortcutSlots.Count >= SlotLimit) return;

            ShortcutSlot slot = Instantiate(ShortcutTemplate, Template.transform);
            slot.gameObject.SetActive(true);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(Offset.x * ShortcutSlots.Count, Offset.y);
            if (!ShowRemoveButton) slot.CombineText.gameObject.SetActive(false);
            else slot.CombineText.gameObject.SetActive(true);
            ShortcutSlots.Insert(0, slot);

            AddButton.anchoredPosition = new Vector2(Offset.x * ShortcutSlots.Count, Offset.y);

            //Save
            Save();
        }

        public void ResetClear()
        {
            Clear();
            AddSlot(false);
        }

        public void Clear()
        {
            ResetButton.SetTrigger("Spin");
            for (int i = 0; i < ShortcutSlots.Count; i++) Destroy(ShortcutSlots[i].gameObject);
            ShortcutSlots.Clear();
        }

        public void Save()
        {
            if (IgnoreSave) return;

            switch (ShortcutField)
            {
                case ShortcutType.SettingsMenu:
                    Calculator.instance.Controls.SettingsMenuKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.SettingsMenuKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.Screenshot:
                    Calculator.instance.Controls.ScreenshotKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.ScreenshotKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.Undo:
                    Calculator.instance.Controls.UndoKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.UndoKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.Redo:
                    Calculator.instance.Controls.RedoKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.RedoKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.Save:
                    Calculator.instance.Controls.SaveKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.SaveKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.SaveAs:
                    Calculator.instance.Controls.SaveAsKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.SaveAsKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.Load:
                    Calculator.instance.Controls.LoadKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.LoadKeys.Add(ShortcutSlots[i].Key);
                    break;
                case ShortcutType.NewFile:
                    Calculator.instance.Controls.NewFileKeys.Clear();
                    for (int i = 0; i < ShortcutSlots.Count; i++) Calculator.instance.Controls.NewFileKeys.Add(ShortcutSlots[i].Key);
                    break;
                default:
                    break;
            }
            Calculator.instance.SaveShortcuts();
        }

        public void Load()
        {
            IgnoreSave = true;
            Clear();
            switch (ShortcutField)
            {
                case ShortcutType.SettingsMenu:
                    for (int i = 0; i < Calculator.instance.Controls.SettingsMenuKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.SettingsMenuKeys[i]);
                    }
                    break;
                case ShortcutType.Screenshot:
                    for (int i = 0; i < Calculator.instance.Controls.ScreenshotKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.ScreenshotKeys[i]);
                    }
                    break;
                case ShortcutType.Undo:
                    for (int i = 0; i < Calculator.instance.Controls.UndoKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.UndoKeys[i]);
                    }
                    break;
                case ShortcutType.Redo:
                    for (int i = 0; i < Calculator.instance.Controls.RedoKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.RedoKeys[i]);
                    }
                    break;
                case ShortcutType.Save:
                    for (int i = 0; i < Calculator.instance.Controls.SaveKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.SaveKeys[i]);
                    }
                    break;
                case ShortcutType.SaveAs:
                    for (int i = 0; i < Calculator.instance.Controls.SaveAsKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.SaveAsKeys[i]);
                    }
                    break;
                case ShortcutType.Load:
                    for (int i = 0; i < Calculator.instance.Controls.LoadKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.LoadKeys[i]);
                    }
                    break;
                case ShortcutType.NewFile:
                    for (int i = 0; i < Calculator.instance.Controls.NewFileKeys.Count; i++)
                    {
                        AddSlot(i != 0);
                        ShortcutSlots[i].SetValue(Calculator.instance.Controls.NewFileKeys[i]);
                    }
                    break;
                default:
                    break;
            }
            IgnoreSave = false;
        }
    }
}
