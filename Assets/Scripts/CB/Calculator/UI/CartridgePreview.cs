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
    /// Preview ui loader for cartridges
    /// </summary>
    public class CartridgePreview : MonoBehaviour
    {
        public CartridgeListView CartridgeLibrary;
        public UIEnableSectionFitTrigger TabFitter;
        public UIEnableSectionFitTrigger ListViewSectionFitter;
        public RectTransform CartridgeTab;

        /*Binds*/
        [Header("Binds")]
        public RectTransform PreviewSection;
        public RectTransform CartridgeSection;
        public TMP_InputField NameBind;
        public TMP_InputField LocationBind;
        public TMP_InputField DateBind;
        //Modifier header
        public TMP_InputField CostBind;
        public TMP_InputField HPBind;
        public TMP_InputField StrBind;
        public TMP_InputField TecBind;
        public TMP_InputField WlkBind;
        public TMP_InputField FlyBind;
        public TMP_InputField TghBind;
        public TMP_InputField CapaBind;
        public TMP_InputField MainForceBind;
        public TMP_InputField MainAmmoBind;
        public TMP_InputField MainRangeBind;
        public TMP_InputField MainSpeedBind;
        public TMP_InputField MainIntBind;
        public TMP_InputField MainForceMultiplierBind;
        public TMP_InputField MainAmmoMultiplierBind;
        public TMP_InputField MainRangeMultiplierBind;
        public TMP_InputField MainSpeedMultiplierBind;
        public TMP_InputField MainIntMultiplierBind;
        public TMP_InputField SubForceBind;
        public TMP_InputField SubAmmoBind;
        public TMP_InputField SubRangeBind;
        public TMP_InputField SubSpeedBind;
        public TMP_InputField SubIntBind;
        public TMP_InputField SubForceMultiplierBind;
        public TMP_InputField SubAmmoMultiplierBind;
        public TMP_InputField SubRangeMultiplierBind;
        public TMP_InputField SubSpeedMultiplierBind;
        public TMP_InputField SubIntMultiplierBind;

        /*Cache*/
        private string CurrentPreviewPath = "";

        void OnEnable()
        {
            //Set listeners
            CartridgeLibrary.OnPreview += OnPreview;
            OnPreview("");
        }

        void OnDisable()
        {
            //Unset listeners
            CartridgeLibrary.OnPreview -= OnPreview;
        }

        public void OnPreview(string path)
        {
            CurrentPreviewPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                if (Calculator.instance.Cartridges.ContainsKey(path))
                {
                    Cartridge cartridge = Calculator.instance.Cartridges[path];

                    PreviewSection.gameObject.SetActive(true);
                    CartridgeSection.gameObject.SetActive(true);
                    NameBind.text = Path.GetFileNameWithoutExtension(path);
                    LocationBind.text = path;
                    DateBind.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
                    CostBind.text = cartridge.Stats.COST.ToString();
                    HPBind.text = cartridge.Stats.HP.ToString();
                    StrBind.text = cartridge.Stats.STR.ToString();
                    TecBind.text = cartridge.Stats.TEC.ToString();
                    WlkBind.text = cartridge.Stats.WLK.ToString();
                    FlyBind.text = cartridge.Stats.FLY.ToString();
                    TghBind.text = cartridge.Stats.TGH.ToString();
                    CapaBind.text = cartridge.Stats.CAPA.ToString();
                    MainForceBind.text = cartridge.MainStats.Force.ToString();
                    MainAmmoBind.text = cartridge.MainStats.Ammo.ToString();
                    MainRangeBind.text = cartridge.MainStats.Range.ToString();
                    MainSpeedBind.text = cartridge.MainStats.Speed.ToString();
                    MainIntBind.text = cartridge.MainStats.Int.ToString();
                    MainForceMultiplierBind.text = cartridge.MainMultiplier.Force.ToString();
                    MainAmmoMultiplierBind.text = cartridge.MainMultiplier.Ammo.ToString();
                    MainRangeMultiplierBind.text = cartridge.MainMultiplier.Range.ToString();
                    MainSpeedMultiplierBind.text = cartridge.MainMultiplier.Speed.ToString();
                    MainIntMultiplierBind.text = cartridge.MainMultiplier.Int.ToString();
                    SubForceBind.text = cartridge.SubStats.Force.ToString();
                    SubAmmoBind.text = cartridge.SubStats.Ammo.ToString();
                    SubRangeBind.text = cartridge.SubStats.Range.ToString();
                    SubSpeedBind.text = cartridge.SubStats.Speed.ToString();
                    SubIntBind.text = cartridge.SubStats.Int.ToString();
                    SubForceMultiplierBind.text = cartridge.SubMultiplier.Force.ToString();
                    SubAmmoMultiplierBind.text = cartridge.SubMultiplier.Ammo.ToString();
                    SubRangeMultiplierBind.text = cartridge.SubMultiplier.Range.ToString();
                    SubSpeedMultiplierBind.text = cartridge.SubMultiplier.Speed.ToString();
                    SubIntMultiplierBind.text = cartridge.SubMultiplier.Int.ToString();
                }
                else
                {
                    CartridgeSection.gameObject.SetActive(false);
                    PreviewSection.gameObject.SetActive(false);
                }
            }
            else
            {
                CartridgeSection.gameObject.SetActive(false);
                PreviewSection.gameObject.SetActive(false);
            }
            TabFitter.Resize();
            ListViewSectionFitter.Resize();
        }

        #region Utils
        public void Edit()
        {
            if (!string.IsNullOrEmpty(CurrentPreviewPath))
            {
                if (Calculator.instance.Cartridges.ContainsKey(CurrentPreviewPath))
                {
                    CartridgeTab.gameObject.SetActive(true);
                    Calculator.instance.CustomCartridgeBuilder.Load(ObjectCloner<Cartridge>.Clone(Calculator.instance.Cartridges[CurrentPreviewPath]));
                    Calculator.instance.SaveLocations[Calculator.instance.SaveState] = CurrentPreviewPath;
                    Calculator.instance.FileNameInputField.text = Path.GetFileNameWithoutExtension(Calculator.instance.SaveLocations[Calculator.instance.SaveState]);
                }
            }
        }
        #endregion
    }
}
