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
    /// Preview ui loader for parts
    /// </summary>
    public class PartPreview : MonoBehaviour
    {
        /*Enum*/
        public enum PreviewType { Build, Part };

        public BuildListView BuildLibrary;
        public PartListView PartLibrary;
        public UIEnableSectionFitTrigger TabFitter;
        public UIEnableSectionFitTrigger ListViewSectionFitter;
        public RectTransform BuildTab;
        public RectTransform PartTab;
        public RectTransform CartridgeContent;
        public PreviewCartridgeSlot CartridgeSlotTemplate;
        public Color CartridgeOnColor;
        public Vector2 CartridgeSlotOffset = new Vector2(0, -28);

        /*Binds*/
        [Header("Binds")]
        public RectTransform PreviewSection;
        public RectTransform PartSection;
        public TMP_InputField NameBind;
        public TMP_InputField LocationBind;
        public TMP_InputField DateBind;
        public TMP_InputField ExBind;
        //Robot Information header
        public TMP_InputField TypeBind;
        public TMP_InputField HPBind;
        public TMP_InputField SizeBind;
        public TMP_InputField CostBind;
        public TMP_InputField CapaBind;
        public TMP_InputField StrBind;
        public TMP_InputField TecBind;
        public TMP_InputField WlkBind;
        public TMP_InputField FlyBind;
        public TMP_InputField TghBind;
        public TMP_InputField StrBarBind;
        public TMP_InputField TecBarBind;
        public TMP_InputField WlkBarBind;
        public TMP_InputField FlyBarBind;
        public TMP_InputField TghBarBind;
        //Cartridge header
        public TMP_InputField LevelBind;

        /*Cache*/
        private string CurrentPreviewPath = "";
        private List<PreviewCartridgeSlot> CartridgeSlots = new List<PreviewCartridgeSlot>();
        private PreviewType PreviewState = PreviewType.Build;

        void OnEnable()
        {
            //Set listeners
            BuildLibrary.OnPreview += OnPreview;
            PartLibrary.OnPreview += OnPreview;
            OnPreview("");
        }

        void OnDisable()
        {
            //Unset listeners
            BuildLibrary.OnPreview -= OnPreview;
            PartLibrary.OnPreview -= OnPreview;
        }

        public void OnPreview(string path)
        {
            CurrentPreviewPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                bool keyCheck = false;
                Contraption contraption = null;
                switch (PreviewState)
                {
                    case PreviewType.Build:
                        if (Calculator.instance.Builds.ContainsKey(path))
                        {
                            keyCheck = true;
                            contraption = Calculator.instance.Builds[path];
                        }
                        break;
                    case PreviewType.Part:
                        if (Calculator.instance.Parts.ContainsKey(path))
                        {
                            keyCheck = true;
                            contraption = Calculator.instance.Parts[path];
                        }
                        break;
                    default:
                        break;
                }

                if (keyCheck)
                {
                    PreviewSection.gameObject.SetActive(true);
                    PartSection.gameObject.SetActive(true);
                    NameBind.text = Path.GetFileNameWithoutExtension(path);
                    LocationBind.text = path;
                    DateBind.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
                    ExBind.text = contraption.ExTuneCount.ToString();
                    UpdateType(contraption);
                    HPBind.text = contraption.TotalStats.HP.ToString();
                    UpdateSize(contraption);
                    CostBind.text = contraption.TotalStats.COST.ToString();
                    CapaBind.text = contraption.TotalStats.CAPA.ToString();
                    UpdateBarStat(contraption.TotalStats.STR, StrBarBind, StrBind);
                    UpdateBarStat(contraption.TotalStats.TEC, TecBarBind, TecBind);
                    UpdateBarStat(contraption.TotalStats.WLK, WlkBarBind, WlkBind);
                    UpdateBarStat(contraption.TotalStats.FLY, FlyBarBind, FlyBind);
                    UpdateBarStat(contraption.TotalStats.TGH, TghBarBind, TghBind);
                    LevelBind.text = contraption.MaxLevel.ToString();

                    ClearCartridgeSlots();
                    for (int i = 0; i < contraption.Cartridges.Count; i++) AddCartridgeSlot(contraption.Cartridges[i]);
                }
                else
                {
                    PartSection.gameObject.SetActive(false);
                    PreviewSection.gameObject.SetActive(false);
                }
            }
            else
            {
                PartSection.gameObject.SetActive(false);
                PreviewSection.gameObject.SetActive(false);
            }
            TabFitter.Resize();
            ListViewSectionFitter.Resize();
        }

        #region Utils
        void UpdateType(Contraption contraption)
        {
            TypeBind.text = "--- | --- | --- | ---";

            if(contraption != null)
            {
                if (contraption.Root.EquipedPart != null)
                {
                    string text = "";
                    for (int i = 0; i < 4; i++)
                    {
                        string typeName = "";
                        switch (i)
                        {
                            case 0:
                                typeName = "Lnd";
                                break;
                            case 1:
                                typeName = "Air";
                                break;
                            case 2:
                                typeName = "Art";
                                break;
                            case 3:
                                typeName = "Sup";
                                break;
                            default:
                                typeName = "Null";
                                break;
                        }

                        if (i > 0 && i < 4) text += " | ";

                        if (contraption.Root.EquipedPart.BDMask.HasFlag(i)) text += typeName;
                        else text += "---";
                    }
                    TypeBind.text = text;
                }
            }
        }

        void UpdateSize(Contraption contraption)
        {
            SizeBind.text = "--";
            if (contraption != null)
            {
                if (contraption.Root.EquipedPart != null) SizeBind.text = contraption.Root.EquipedPart.Size == Part.SizeType.None ? "--" : contraption.Root.EquipedPart.Size.ToString();
            }
        }

        void UpdateBarStat(int stat, TMP_InputField statBarBind, TMP_InputField statBind)
        {
            string text = "";
            for (int i = 0; i < Mathf.Clamp(stat, 0, 40); i++)
            {
                if ((i + 1) % 10 == 0) text += "l";
                else text += "I";
            }
            statBarBind.text = text;
            statBind.text = stat.ToString();
        }

        void AddCartridgeSlot((Cartridge, int, bool) cartridge)
        {
            PreviewCartridgeSlot previewCartridgeSlot = Instantiate(CartridgeSlotTemplate, CartridgeContent.transform);
            previewCartridgeSlot.gameObject.SetActive(true);
            previewCartridgeSlot.TitleInputField.text = cartridge.Item1.Name;
            previewCartridgeSlot.CostInputField.text = cartridge.Item2.ToString();
            if (cartridge.Item3) previewCartridgeSlot.BackgroundImage.color = CartridgeOnColor;
            previewCartridgeSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(CartridgeSlotOffset.x, CartridgeSlotOffset.y * CartridgeSlots.Count);
            CartridgeSlots.Add(previewCartridgeSlot);

            CartridgeContent.sizeDelta = new Vector2(CartridgeContent.sizeDelta.x, -CartridgeSlotOffset.y * CartridgeSlots.Count);
        }

        void ClearCartridgeSlots()
        {
            for (int i = 0; i < CartridgeSlots.Count; i++) Destroy(CartridgeSlots[i].gameObject);
            CartridgeSlots.Clear();
        }

        public void Edit()
        {
            if (!string.IsNullOrEmpty(CurrentPreviewPath))
            {
                bool keyCheck = false;
                Contraption contraption = null;
                RectTransform tab = null;
                IBuilder<Contraption> builder = null;
                switch (PreviewState)
                {
                    case PreviewType.Build:
                        if (Calculator.instance.Builds.ContainsKey(CurrentPreviewPath))
                        {
                            keyCheck = true;
                            contraption = Calculator.instance.Builds[CurrentPreviewPath];
                            tab = BuildTab;
                            builder = Calculator.instance.CustomBuildBuilder;
                        }
                        break;
                    case PreviewType.Part:
                        if (Calculator.instance.Parts.ContainsKey(CurrentPreviewPath))
                        {
                            keyCheck = true;
                            contraption = Calculator.instance.Parts[CurrentPreviewPath];
                            tab = PartTab;
                            builder = Calculator.instance.CustomPartBuilder;
                        }
                        break;
                    default:
                        break;
                }

                if (keyCheck)
                {
                    tab.gameObject.SetActive(true);
                    builder.Load(ObjectCloner<Contraption>.Clone(contraption));
                    Calculator.instance.SaveLocations[Calculator.instance.SaveState] = CurrentPreviewPath;
                    Calculator.instance.FileNameInputField.text = Path.GetFileNameWithoutExtension(Calculator.instance.SaveLocations[Calculator.instance.SaveState]);
                }
            }
        }

        public void SetPreviewState(int state)
        {
            PreviewState = (PreviewType)state;
        }
        #endregion
    }
}
