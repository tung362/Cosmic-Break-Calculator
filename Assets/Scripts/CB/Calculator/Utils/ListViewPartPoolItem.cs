using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Utils;

namespace CB.Calculator.Utils
{
    /// <summary>
    /// List view pool item for parts
    /// </summary>
    public class ListViewPartPoolItem : IPool<string>
    {
        public Toggle SelectToggle;
        public Button BackgroundButton;
        public TMP_Text NameText;
        public TMP_Text COSTText;
        public TMP_Text CAPAText;
        public TMP_Text HPText;
        public TMP_Text STRText;
        public TMP_Text TECText;
        public TMP_Text WLKText;
        public TMP_Text FLYText;
        public TMP_Text TGHText;
        public TMP_Text JointText;
        public TMP_Text SizeText;
        public TMP_Text MaxLevelText;
        public TMP_Text ExTuneCountText;
        public TMP_Text LocationText;
        public TMP_Text DateText;
        public bool Enabled = true;

        public ListViewPartPoolItem(List<RectTransform> itemFields)
        {
            SelectToggle = itemFields[0].GetComponent<Toggle>();
            BackgroundButton = itemFields[1].GetComponent<Button>();
            NameText = itemFields[2].GetComponent<TMP_Text>();
            COSTText = itemFields[3].GetComponent<TMP_Text>();
            CAPAText = itemFields[4].GetComponent<TMP_Text>();
            HPText = itemFields[5].GetComponent<TMP_Text>();
            STRText = itemFields[6].GetComponent<TMP_Text>();
            TECText = itemFields[7].GetComponent<TMP_Text>();
            WLKText = itemFields[8].GetComponent<TMP_Text>();
            FLYText = itemFields[9].GetComponent<TMP_Text>();
            TGHText = itemFields[10].GetComponent<TMP_Text>();
            JointText = itemFields[11].GetComponent<TMP_Text>();
            SizeText = itemFields[12].GetComponent<TMP_Text>();
            MaxLevelText = itemFields[13].GetComponent<TMP_Text>();
            ExTuneCountText = itemFields[14].GetComponent<TMP_Text>();
            LocationText = itemFields[15].GetComponent<TMP_Text>();
            DateText = itemFields[16].GetComponent<TMP_Text>();
        }

        public void SetActive(bool toggle)
        {
            if (Enabled == toggle) return;

            SelectToggle.gameObject.SetActive(toggle);
            BackgroundButton.gameObject.SetActive(toggle);
            NameText.gameObject.SetActive(toggle);
            COSTText.gameObject.SetActive(toggle);
            CAPAText.gameObject.SetActive(toggle);
            HPText.gameObject.SetActive(toggle);
            STRText.gameObject.SetActive(toggle);
            TECText.gameObject.SetActive(toggle);
            WLKText.gameObject.SetActive(toggle);
            FLYText.gameObject.SetActive(toggle);
            TGHText.gameObject.SetActive(toggle);
            JointText.gameObject.SetActive(toggle);
            SizeText.gameObject.SetActive(toggle);
            MaxLevelText.gameObject.SetActive(toggle);
            ExTuneCountText.gameObject.SetActive(toggle);
            LocationText.gameObject.SetActive(toggle);
            DateText.gameObject.SetActive(toggle);
            Enabled = toggle;
        }

        public void UpdateItem(string path)
        {
            if (!Calculator.instance.Parts.ContainsKey(path)) return;

            NameText.text = Path.GetFileNameWithoutExtension(path);
            COSTText.text = Calculator.instance.Parts[path].TotalStats.COST.ToString();
            CAPAText.text = Calculator.instance.Parts[path].TotalStats.CAPA.ToString();
            HPText.text = Calculator.instance.Parts[path].TotalStats.HP.ToString();
            STRText.text = Calculator.instance.Parts[path].TotalStats.STR.ToString();
            TECText.text = Calculator.instance.Parts[path].TotalStats.TEC.ToString();
            WLKText.text = Calculator.instance.Parts[path].TotalStats.WLK.ToString();
            FLYText.text = Calculator.instance.Parts[path].TotalStats.FLY.ToString();
            TGHText.text = Calculator.instance.Parts[path].TotalStats.TGH.ToString();
            JointText.text = "-";
            SizeText.text = "-";
            MaxLevelText.text = Calculator.instance.Parts[path].MaxLevel.ToString();
            ExTuneCountText.text = Calculator.instance.Parts[path].ExTuneCount.ToString();
            LocationText.text = path;
            DateText.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");

            if (Calculator.instance.Parts[path].Root.EquipedPart != null)
            {
                JointText.text = Calculator.instance.Parts[path].Root.Joint.ToString();
                SizeText.text = Calculator.instance.Parts[path].Root.EquipedPart.Size.ToString();
            }
        }

        public void UpdateItem(string path, bool selected)
        {
            SelectToggle.isOn = selected;
            UpdateItem(path);
        }

        public void Destroy()
        {
            Object.Destroy(SelectToggle.gameObject);
            Object.Destroy(BackgroundButton.gameObject);
            Object.Destroy(NameText.gameObject);
            Object.Destroy(COSTText.gameObject);
            Object.Destroy(CAPAText.gameObject);
            Object.Destroy(HPText.gameObject);
            Object.Destroy(STRText.gameObject);
            Object.Destroy(TECText.gameObject);
            Object.Destroy(WLKText.gameObject);
            Object.Destroy(FLYText.gameObject);
            Object.Destroy(TGHText.gameObject);
            Object.Destroy(JointText.gameObject);
            Object.Destroy(SizeText.gameObject);
            Object.Destroy(MaxLevelText.gameObject);
            Object.Destroy(ExTuneCountText.gameObject);
            Object.Destroy(LocationText.gameObject);
            Object.Destroy(DateText.gameObject);
        }
    }
}
