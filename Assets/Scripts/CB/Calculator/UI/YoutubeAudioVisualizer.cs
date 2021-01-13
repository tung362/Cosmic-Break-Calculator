using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Utils;

namespace CB.Calculator.UI
{
    /// <summary>
    /// UI audio visualizer for videos played
    /// </summary>
    public class YoutubeAudioVisualizer : MonoBehaviour
    {
        public int SpectrumSample = 512; //Must be power of 2
        public int SegmentCount = 8;
        public float BaseHighest = 1.0f;
        public float MaxSize = 30.0f;
        public float DropRate = 0.005f;
        public float DropMultiplier = 1.2f;

        public RectTransform Icon;
        public RectTransform MiddleSegment;
        public RectTransform Icon2;
        public RectTransform Segment;

        //public GameObject Icon;
        //public GameObject MiddleSegment;
        //public GameObject Icon2;
        //public GameObject Segment;

        /*Cache*/
        [HideInInspector]
        public float[] Output;
        [HideInInspector]
        public float AmplitudeOutput = 0;
        private float[] PreviousOutput;
        private float[] DropBuffer;
        private float[] Highest;
        private float AmplitudeHighest = 0;
        [HideInInspector]
        public List<RectTransform> Segments = new List<RectTransform>();
        [HideInInspector]
        public List<RectTransform> Segments2 = new List<RectTransform>();
        //[HideInInspector]
        //public List<GameObject> Segments = new List<GameObject>();
        //[HideInInspector]
        //public List<GameObject> Segments2 = new List<GameObject>();
        void Start()
        {
            Output = new float[SegmentCount];
            PreviousOutput = new float[SegmentCount];
            DropBuffer = new float[SegmentCount];
            Highest = new float[SegmentCount];

            for (int i = 0; i < SegmentCount; i++) Highest[i] = BaseHighest;

            float StartingX = -((SegmentCount * 6.0f) * 0.5f);
            for (int i = 0; i < SegmentCount; i++)
            {
                RectTransform segment = Instantiate(MiddleSegment, Icon.transform);
                segment.gameObject.SetActive(true);
                segment.anchoredPosition = new Vector2(StartingX, 0);
                StartingX += 6.0f;
                Segments.Add(segment);
            }

            for (int i = 0; i < 30; i++)
            {
                RectTransform segment = Instantiate(Segment, Icon2.transform);
                segment.gameObject.SetActive(true);
                segment.anchoredPosition = new Vector2(i * 6, 0);
                Segments2.Add(segment);
            }
            //float StartingX = -((SegmentCount * 0.5f) * 0.5f);
            //for (int i = 0; i < SegmentCount; i++)
            //{
            //    GameObject segment = Instantiate(MiddleSegment, Icon.transform);
            //    segment.gameObject.SetActive(true);
            //    segment.transform.localPosition = new Vector3(StartingX, 0, 0);
            //    StartingX += 6.0f;
            //    Segments.Add(segment);
            //}

            //for (int i = 0; i < 30; i++)
            //{
            //    GameObject segment = Instantiate(Segment, Icon2.transform);
            //    segment.gameObject.SetActive(true);
            //    segment.transform.localPosition = new Vector3(i * 0.5f, 0, 0);
            //    Segments2.Add(segment);
            //}
        }

        void Update()
        {
            float[] rawOutput = new float[SegmentCount];
            float[] spectrum = new float[SpectrumSample];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            AudioVisualizer.CalculateRawSegments(spectrum, ref Output);
            Output.CopyTo(rawOutput, 0);
            AudioVisualizer.CalculateDrop(rawOutput, ref Output, ref PreviousOutput, ref DropBuffer, DropRate, DropMultiplier);
            AudioVisualizer.NormalizedSegments(ref Output, ref rawOutput, ref Highest);
            AudioVisualizer.CreateAmplitude(rawOutput, Output, ref AmplitudeOutput, ref AmplitudeHighest);
            AudioVisualizer.DebugOutput(Output, MaxSize);
            AudioVisualizer.DebugAmplitude(Output, AmplitudeOutput, MaxSize);

            //Icon.localScale = new Vector3(Mathf.Lerp(1, 3, Output[0]), Mathf.Lerp(1, 3, Output[0]), 1);
            for (int i = 0; i < SegmentCount; i++)
            {
                Segments[i].sizeDelta = new Vector2(4, Mathf.Lerp(0, 68, Output[i]));
            }

            for (int i = 0; i < Segments2.Count; i++)
            {
                Segments2[i].sizeDelta = new Vector2(4, Mathf.Lerp(0, 26.4f, Output[i]));
            }

            //for (int i = 0; i < SegmentCount; i++)
            //{
            //    Segments[i].transform.localScale = new Vector3(1, Mathf.Lerp(0, 2.0f, Output[i]), 1);
            //}

            //for (int i = 0; i < Segments2.Count; i++)
            //{
            //    Segments2[i].transform.localScale = new Vector3(1, Mathf.Lerp(0, 2.0f, Output[i]), 1);
            //}
        }
    }
}
