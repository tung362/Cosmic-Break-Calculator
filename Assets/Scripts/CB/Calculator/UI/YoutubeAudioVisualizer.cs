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

        public GameObject MiddleContent;
        public GameObject MiddleSegmentTemplate;
        public GameObject TopContent;
        public GameObject TopSegmentTemplate;

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
        public List<GameObject> Segments = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> Segments2 = new List<GameObject>();

        void Start()
        {
            Output = new float[SegmentCount];
            PreviousOutput = new float[SegmentCount];
            DropBuffer = new float[SegmentCount];
            Highest = new float[SegmentCount];

            for (int i = 0; i < SegmentCount; i++) Highest[i] = BaseHighest;

            float StartingX = -((SegmentCount * 0.3f) * 0.5f);
            for (int i = 0; i < SegmentCount; i++)
            {
                GameObject segment = Instantiate(MiddleSegmentTemplate, MiddleContent.transform);
                segment.gameObject.SetActive(true);
                segment.transform.localPosition = new Vector3(StartingX, 0, 0);
                StartingX += 0.3f;
                Segments.Add(segment);
            }

            for (int i = 0; i < 30; i++)
            {
                GameObject segment = Instantiate(TopSegmentTemplate, TopContent.transform);
                segment.gameObject.SetActive(true);
                segment.transform.localPosition = new Vector3(i * 0.3f, 0, 0);
                Segments2.Add(segment);
            }
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
            //AudioVisualizer.DebugOutput(Output, MaxSize);
            //AudioVisualizer.DebugAmplitude(Output, AmplitudeOutput, MaxSize);

            for (int i = 0; i < SegmentCount; i++) Segments[i].transform.localScale = new Vector3(0.25f, Mathf.Lerp(0, 3.66f, Output[i]), 0);
            for (int i = 0; i < Segments2.Count; i++) Segments2[i].transform.localScale = new Vector3(0.25f, Mathf.Lerp(0, 1.5f, Output[i]), 0);
        }
    }
}
