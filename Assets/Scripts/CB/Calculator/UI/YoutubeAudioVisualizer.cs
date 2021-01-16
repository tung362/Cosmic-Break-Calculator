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
        public int SegmentCount = 190;
        public int TopSegmentCount = 30;
        public float BaseHighest = 1.0f;
        public float DropRate = 0.005f;
        public float DropMultiplier = 1.2f;
        public GameObject MiddleContent;
        public GameObject MiddleSegmentTemplate;
        public GameObject TopContent;
        public GameObject TopSegmentTemplate;
        public float Offset = 0.3f;
        public float MaxSize = 3.66f;
        public float MaxSizeTop = 1.5f;

        /*Cache*/
        [HideInInspector]
        public float[] Output;
        [HideInInspector]
        public float AmplitudeOutput = 0;
        private float[] PreviousOutput;
        private float[] DropBuffer;
        private float[] Highest;
        private float AmplitudeHighest = 0;
        private List<GameObject> MiddleSegments = new List<GameObject>();
        private List<GameObject> TopSegments = new List<GameObject>();
        //private bool Update

        void Start()
        {
            Output = new float[SegmentCount];
            PreviousOutput = new float[SegmentCount];
            DropBuffer = new float[SegmentCount];
            Highest = new float[SegmentCount];

            for (int i = 0; i < SegmentCount; i++) Highest[i] = BaseHighest;

            //Generate segments
            TopSegmentCount = Mathf.Clamp(TopSegmentCount, 0, SegmentCount);
            float StartingX = -((SegmentCount * Offset) * 0.5f);
            for (int i = 0; i < SegmentCount; i++)
            {
                GameObject segment = Instantiate(MiddleSegmentTemplate, MiddleContent.transform);
                segment.gameObject.SetActive(true);
                segment.transform.localPosition = new Vector3(StartingX, 0, 0);
                StartingX += Offset;
                MiddleSegments.Add(segment);
            }

            for (int i = 0; i < TopSegmentCount; i++)
            {
                GameObject segment = Instantiate(TopSegmentTemplate, TopContent.transform);
                segment.gameObject.SetActive(true);
                segment.transform.localPosition = new Vector3(i * Offset, 0, 0);
                TopSegments.Add(segment);
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
            //AudioVisualizer.DebugOutput(Output, 30.0f);
            //AudioVisualizer.DebugAmplitude(Output, AmplitudeOutput, 30.0f);

            //Sync segments with audio
            for (int i = 0; i < SegmentCount; i++)
            {
                MiddleSegments[i].transform.localScale = new Vector3(MiddleSegments[i].transform.localScale.x, Mathf.Lerp(0, MaxSize, Output[i]), MiddleSegments[i].transform.localScale.z);
                if(i < TopSegments.Count) TopSegments[i].transform.localScale = new Vector3(TopSegments[i].transform.localScale.x, Mathf.Lerp(0, MaxSizeTop, Output[i]), TopSegments[i].transform.localScale.z);
            }
        }
    }
}
