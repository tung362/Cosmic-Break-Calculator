using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.UI;
using CB.Utils;

namespace CB.Calculator.UI
{
    /// <summary>
    /// Preview ui loader for builds, parts, tunes, and cartridges
    /// </summary>
    public class AllPreview : MonoBehaviour
    {
        public AllListView AllLibrary;
        public PartPreview PartFilePreview;
        public TunePreview TuneFilePreview;
        public CartridgePreview CartridgeFilePreview;

        void OnEnable()
        {
            //Set listeners
            AllLibrary.OnPreview += OnPreview;
            OnPreview("", Calculator.SaveType.None);
        }

        void OnDisable()
        {
            //Unset listeners
            AllLibrary.OnPreview -= OnPreview;
        }

        void OnPreview(string path, Calculator.SaveType saveType)
        {
            switch (saveType)
            {
                case Calculator.SaveType.Build:
                    PartFilePreview.SetPreviewState(0);
                    PartFilePreview.OnPreview(path);
                    break;
                case Calculator.SaveType.Part:
                    PartFilePreview.SetPreviewState(1);
                    PartFilePreview.OnPreview(path);
                    break;
                case Calculator.SaveType.Tune:
                    TuneFilePreview.OnPreview(path);
                    break;
                case Calculator.SaveType.Cartridge:
                    CartridgeFilePreview.OnPreview(path);
                    break;
                default:
                    PartFilePreview.OnPreview(path);
                    break;
            }
        }
    }
}
