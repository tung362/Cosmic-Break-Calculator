using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Hook for sending updates with shortcut keys to the UIHintBind
    /// UIHintBind must be set up
    /// </summary>
    public class UIShortcutHintHook : UIHintHook
    {
        public ShortcutsEdit.ShortcutType ShortcutField;
        public bool EnterAdd = true;
        public bool ExitAdd = false;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UIHintBind.instance.UpdateText(EnterAdd ? EnterText + GetShortcutText() : EnterText);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UIHintBind.instance.UpdateText(ExitAdd ? ExitText + GetShortcutText() : ExitText);
        }

        string GetShortcutText()
        {
            List<KeyCode> keys = new List<KeyCode>();
            switch (ShortcutField)
            {
                case ShortcutsEdit.ShortcutType.SettingsMenu:
                    keys = Calculator.instance.Controls.SettingsMenuKeys;
                    break;
                case ShortcutsEdit.ShortcutType.Screenshot:
                    keys = Calculator.instance.Controls.ScreenshotKeys;
                    break;
                case ShortcutsEdit.ShortcutType.Undo:
                    keys = Calculator.instance.Controls.UndoKeys;
                    break;
                case ShortcutsEdit.ShortcutType.Redo:
                    keys = Calculator.instance.Controls.RedoKeys;
                    break;
                case ShortcutsEdit.ShortcutType.Save:
                    keys = Calculator.instance.Controls.SaveKeys;
                    break;
                case ShortcutsEdit.ShortcutType.SaveAs:
                    keys = Calculator.instance.Controls.SaveAsKeys;
                    break;
                case ShortcutsEdit.ShortcutType.Load:
                    keys = Calculator.instance.Controls.LoadKeys;
                    break;
                case ShortcutsEdit.ShortcutType.NewFile:
                    keys = Calculator.instance.Controls.NewFileKeys;
                    break;
                default:
                    break;
            }
            string shortcutText = "";
            if (keys.Count != 0)
            {
                shortcutText = " [";
                for (int i = 0; i < keys.Count; i++)
                {
                    shortcutText += keys[i].ToString();
                    if (i < keys.Count - 1) shortcutText += '+';
                }
                shortcutText += ']';
            }
            return shortcutText;
        }
    }
}
