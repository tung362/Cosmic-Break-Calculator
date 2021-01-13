using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Utils
{
    /// <summary>
    /// Audio visualizer calculation utilities
    /// </summary>
    public static class AudioVisualizer
    {
        public static void CreateAmplitude(float[] rawOutput, float[] output, ref float amplitudeOutput, ref float amplitudeHighest)
        {
            //Get sum
            float currentRawAmplitude = 0;
            float currentAmplitude = 0;
            for (int i = 0; i < output.Length; i++)
            {
                currentRawAmplitude += rawOutput[i];
                currentAmplitude += output[i];
            }

            //Find highest
            if (currentRawAmplitude > amplitudeHighest) amplitudeHighest = currentRawAmplitude;

            //Result
            amplitudeOutput = currentAmplitude / amplitudeHighest;
        }

        public static void NormalizedSegments(ref float[] output, ref float[] rawOutput, ref float[] highest)
        {
            for (int i = 0; i < output.Length; i++)
            {
                //Get highest
                if (rawOutput[i] > highest[i]) highest[i] = rawOutput[i];
                //Normalize
                rawOutput[i] /= highest[i];
                output[i] /= highest[i];
            }
        }

        public static void CalculateDrop(float[] rawOutput, ref float[] output, ref float[] previousOutput, ref float[] dropBuffer, float dropRate = 0.005f, float dropMultiplier = 1.2f)
        {
            for (int i = 0; i < output.Length; i++)
            {
                //Set buffer at true value and reset drop speed
                if(rawOutput[i] > previousOutput[i])
                {
                    previousOutput[i] = rawOutput[i];
                    dropBuffer[i] = dropRate;
                }

                //Drop buffer and multiply drop speed
                if (rawOutput[i] < previousOutput[i])
                {
                    previousOutput[i] -= dropBuffer[i];
                    if (previousOutput[i] < 0) previousOutput[i] = 0;
                    dropBuffer[i] *= dropMultiplier;
                    output[i] = previousOutput[i];
                }
            }
        }

        public static void CalculateRawSegments(float[] spectrum, ref float[] output)
        {
            //Get average of each segment
            int SamplesPerSegment = spectrum.Length / output.Length;
            int spectrumIndex = 0;
            for(int i = 0; i < output.Length; i++)
            {
                //Get sum of each sample in the segment
                float sum = 0;
                for (int j = 0; j < SamplesPerSegment; j++)
                {
                    sum += spectrum[spectrumIndex] * (spectrumIndex + 1);
                    spectrumIndex++;
                }
                sum /= SamplesPerSegment;
                output[i] = sum;
            }
        }

        public static void DebugOutput(float[] output, float maxSize = 30.0f)
        {
            //Draw lines in editor
            for (int i = 0; i < output.Length - 1; i++)
            {
                Vector3 start = new Vector3(i, -20, 0);
                Vector3 end = new Vector3(i, -20 + Mathf.Lerp(0, maxSize, output[i]), 0);
                Debug.DrawLine(start, end, Color.cyan);
            }
        }

        public static void DebugAmplitude(float[] output, float amplitudeOutput, float MaxSize = 30.0f)
        {
            //Draw lines in editor
            for (int i = 0; i < output.Length - 1; i++)
            {
                Vector3 start = new Vector3(i, -60, 0);
                Vector3 end = new Vector3(i, -60 + Mathf.Lerp(0, MaxSize, amplitudeOutput), 0);
                Debug.DrawLine(start, end, Color.cyan);
            }
        }
    }
}
