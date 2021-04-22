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
    /// List view pool item for tunes
    /// </summary>
    public class ListViewTunePoolItem : IPool<string>
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
        public TMP_Text ExText;
        public TMP_Text SuccessRateText;
        public TMP_Text LocationText;
        public TMP_Text DateText;
        public bool Enabled = true;

        public ListViewTunePoolItem(List<RectTransform> itemFields)
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
            ExText = itemFields[11].GetComponent<TMP_Text>();
            SuccessRateText = itemFields[12].GetComponent<TMP_Text>();
            LocationText = itemFields[13].GetComponent<TMP_Text>();
            DateText = itemFields[14].GetComponent<TMP_Text>();
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
            ExText.gameObject.SetActive(toggle);
            SuccessRateText.gameObject.SetActive(toggle);
            LocationText.gameObject.SetActive(toggle);
            DateText.gameObject.SetActive(toggle);
            Enabled = toggle;
        }

        public void UpdateItem(string path)
        {
            if (!Calculator.instance.Tunes.ContainsKey(path)) return;

            NameText.text = Path.GetFileNameWithoutExtension(path);
            COSTText.text = Calculator.instance.Tunes[path].Stats.COST.ToString();
            CAPAText.text = Calculator.instance.Tunes[path].Stats.CAPA.ToString();
            HPText.text = Calculator.instance.Tunes[path].Stats.HP.ToString();
            STRText.text = Calculator.instance.Tunes[path].Stats.STR.ToString();
            TECText.text = Calculator.instance.Tunes[path].Stats.TEC.ToString();
            WLKText.text = Calculator.instance.Tunes[path].Stats.WLK.ToString();
            FLYText.text = Calculator.instance.Tunes[path].Stats.FLY.ToString();
            TGHText.text = Calculator.instance.Tunes[path].Stats.TGH.ToString();
            ExText.text = Calculator.instance.Tunes[path].IsEx.ToString();
            SuccessRateText.text = Calculator.instance.Tunes[path].SuccessRate.ToString();
            LocationText.text = path;
            DateText.text = File.GetLastWriteTime(path).ToString("MMM d, yyyy");
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
            Object.Destroy(ExText.gameObject);
            Object.Destroy(SuccessRateText.gameObject);
            Object.Destroy(LocationText.gameObject);
            Object.Destroy(DateText.gameObject);
        }
    }
}
