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
    /// Preview ui loader for tunes
    /// </summary>
    public class TunePreview : MonoBehaviour
    {
        public TuneListView TuneLibrary;
        public UIEnableSectionFitTrigger TabFitter;
        public UIEnableSectionFitTrigger ListViewSectionFitter;
        public RectTransform TuneTab;
        public RectTransform CraftMaterialContent;
        public PreviewCraftMaterialSlot CraftMaterialSlotTemplate;
        public Vector2 CraftMaterialSlotOffset = new Vector2(0, -28);

        /*Binds*/
        [Header("Binds")]
        public RectTransform PreviewSection;
        public RectTransform TuneSection;
        public TMP_InputField NameBind;
        public TMP_InputField LocationBind;
        public TMP_InputField DateBind;
        public TMP_InputField ExBind;
        public Image ColorBind;
        public TMP_InputField SuccessRateBind;
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
        private List<PreviewCraftMaterialSlot> CraftMaterialSlots = new List<PreviewCraftMaterialSlot>();

        void OnEnable()
        {
            //Set listeners
            TuneLibrary.OnPreview += OnPreview;
            OnPreview("");
        }

        void OnDisable()
        {
            //Unset listeners
            TuneLibrary.OnPreview -= OnPreview;
        }

        public void OnPreview(string path)
        {
            CurrentPreviewPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                if (Calculator.instance.Tunes.ContainsKey(path))
                {
                    Tune tune = Calculator.instance.Tunes[path];

                    PreviewSection.gameObject.SetActive(true);
                    TuneSection.gameObject.SetActive(true);
                    NameBind.text = Path.GetFileNameWithoutExtension(path);
                    LocationBind.text = path;
                    DateBind.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
                    ExBind.text = tune.IsEx.ToString();
                    ColorBind.color = tune.TuneColor;
                    SuccessRateBind.text = tune.SuccessRate.ToString();
                    CostBind.text = tune.Stats.COST.ToString();
                    HPBind.text = tune.Stats.HP.ToString();
                    StrBind.text = tune.Stats.STR.ToString();
                    TecBind.text = tune.Stats.TEC.ToString();
                    WlkBind.text = tune.Stats.WLK.ToString();
                    FlyBind.text = tune.Stats.FLY.ToString();
                    TghBind.text = tune.Stats.TGH.ToString();
                    CapaBind.text = tune.Stats.CAPA.ToString();
                    MainForceBind.text = tune.MainStats.Force.ToString();
                    MainAmmoBind.text = tune.MainStats.Ammo.ToString();
                    MainRangeBind.text = tune.MainStats.Range.ToString();
                    MainSpeedBind.text = tune.MainStats.Speed.ToString();
                    MainIntBind.text = tune.MainStats.Int.ToString();
                    MainForceMultiplierBind.text = tune.MainMultiplier.Force.ToString();
                    MainAmmoMultiplierBind.text = tune.MainMultiplier.Ammo.ToString();
                    MainRangeMultiplierBind.text = tune.MainMultiplier.Range.ToString();
                    MainSpeedMultiplierBind.text = tune.MainMultiplier.Speed.ToString();
                    MainIntMultiplierBind.text = tune.MainMultiplier.Int.ToString();
                    SubForceBind.text = tune.SubStats.Force.ToString();
                    SubAmmoBind.text = tune.SubStats.Ammo.ToString();
                    SubRangeBind.text = tune.SubStats.Range.ToString();
                    SubSpeedBind.text = tune.SubStats.Speed.ToString();
                    SubIntBind.text = tune.SubStats.Int.ToString();
                    SubForceMultiplierBind.text = tune.SubMultiplier.Force.ToString();
                    SubAmmoMultiplierBind.text = tune.SubMultiplier.Ammo.ToString();
                    SubRangeMultiplierBind.text = tune.SubMultiplier.Range.ToString();
                    SubSpeedMultiplierBind.text = tune.SubMultiplier.Speed.ToString();
                    SubIntMultiplierBind.text = tune.SubMultiplier.Int.ToString();
                    ClearCraftMaterialSlots();
                    for (int i = 0; i < tune.CraftMaterials.Count; i++) AddCraftMaterialSlot(tune.CraftMaterials[i]);
                }
                else
                {
                    TuneSection.gameObject.SetActive(false);
                    PreviewSection.gameObject.SetActive(false);
                }
            }
            else
            {
                TuneSection.gameObject.SetActive(false);
                PreviewSection.gameObject.SetActive(false);
            }
            TabFitter.Resize();
            ListViewSectionFitter.Resize();
        }

        #region Utils
        void AddCraftMaterialSlot(Tune.CraftMaterial craftMaterial)
        {
            PreviewCraftMaterialSlot previewCraftMaterialSlot = Instantiate(CraftMaterialSlotTemplate, CraftMaterialContent.transform);
            previewCraftMaterialSlot.gameObject.SetActive(true);
            previewCraftMaterialSlot.NameInputField.text = craftMaterial.Name;
            previewCraftMaterialSlot.AmountInputField.text = craftMaterial.Amount.ToString();
            previewCraftMaterialSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(CraftMaterialSlotOffset.x, CraftMaterialSlotOffset.y * CraftMaterialSlots.Count);
            CraftMaterialSlots.Add(previewCraftMaterialSlot);

            CraftMaterialContent.sizeDelta = new Vector2(CraftMaterialContent.sizeDelta.x, -CraftMaterialSlotOffset.y * CraftMaterialSlots.Count);
        }

        void ClearCraftMaterialSlots()
        {
            for (int i = 0; i < CraftMaterialSlots.Count; i++) Destroy(CraftMaterialSlots[i].gameObject);
            CraftMaterialSlots.Clear();
        }

        public void Edit()
        {
            if (!string.IsNullOrEmpty(CurrentPreviewPath))
            {
                if (Calculator.instance.Tunes.ContainsKey(CurrentPreviewPath))
                {
                    TuneTab.gameObject.SetActive(true);
                    Calculator.instance.CustomTuneBuilder.Load(ObjectCloner<Tune>.Clone(Calculator.instance.Tunes[CurrentPreviewPath]));
                    Calculator.instance.SaveLocations[Calculator.instance.SaveState] = CurrentPreviewPath;
                    Calculator.instance.FileNameInputField.text = Path.GetFileNameWithoutExtension(Calculator.instance.SaveLocations[Calculator.instance.SaveState]);
                }
            }
        }
        #endregion
    }
}
