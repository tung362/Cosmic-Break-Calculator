using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CB.UI
{
    /// <summary>
    /// Generates graph bars for sliders
    /// </summary>
    [ExecuteInEditMode]
    public class UISliderGraphBar : MonoBehaviour
    {
        public Slider SliderBind;
        public RectTransform Template;
        public RectTransform TopBarTemplate;
        public RectTransform BottomBarTemplate;
        public int Segments = 5;
        public int CharacterLimit = 4;

        [HideInInspector]
        public List<RectTransform> GraphBars = new List<RectTransform>();

        public void Generate()
        {
            Clear();
            //Create each segment
            RectTransform sliderTransform = SliderBind.GetComponent<RectTransform>();
            float offset = sliderTransform.sizeDelta.x / (float)Segments;
            for(int i = 0; i < Segments + 1; i++)
            {
                RectTransform template = i % 2 == 0 ? TopBarTemplate : BottomBarTemplate;
                RectTransform graphBar = Instantiate(template, Template);
                graphBar.gameObject.SetActive(true);
                graphBar.anchoredPosition = new Vector2(i * offset, template.anchoredPosition.y);

                //Apply text should there be an input field attacted to the graph bar
                TMP_InputField barInputDield = graphBar.GetComponentInChildren<TMP_InputField>();
                string text = new string(Mathf.Lerp(SliderBind.minValue, SliderBind.maxValue, (offset * i) / sliderTransform.sizeDelta.x).ToString().Take(CharacterLimit).ToArray());
                if (barInputDield) barInputDield.text = text;

                GraphBars.Add(graphBar);
            }
        }

        public void Clear()
        {
            if (!Application.isPlaying)
            {
                for (int i = 0; i < GraphBars.Count; i++)
                {
                    if(GraphBars[i]) DestroyImmediate(GraphBars[i].gameObject);
                }
            }
            else
            {
                for (int i = 0; i < GraphBars.Count; i++)
                {
                    if (GraphBars[i]) Destroy(GraphBars[i].gameObject);
                }
            }
            GraphBars.Clear();
        }
    }
}
