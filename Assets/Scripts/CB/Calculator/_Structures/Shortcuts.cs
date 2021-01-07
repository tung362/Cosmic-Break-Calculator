using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace CB.Calculator
{
    /// <summary>
    /// Calculator control settings
    /// </summary>
    [MessagePackObject]
    public class Shortcuts
    {
        /*General*/
        [Key(0)]
        public List<KeyCode> SettingsMenuKeys = new List<KeyCode>();
        [Key(1)]
        public List<KeyCode> ScreenshotKeys = new List<KeyCode>();
        /*Builder*/
        [Key(2)]
        public List<KeyCode> SaveKeys = new List<KeyCode>();
        [Key(3)]
        public List<KeyCode> UndoKeys = new List<KeyCode>();
        [Key(4)]
        public List<KeyCode> RedoKeys = new List<KeyCode>();
    }
}
