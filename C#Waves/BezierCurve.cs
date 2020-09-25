using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace waves
{
    public class BezierCurve
    {
        fPoint[] samples; //An array of 'fPoints' containing all sample information.
        fPoint[] points;  //An array of 'fPoints' containing all of the points to build the curve.
        int numSamples;   //Total number of samples in the curve.
        int numPoints;    //Total number of points, minimum 3 (could be 2, but that would result in a linear line)
        bool audioMapped;

        public BezierCurve(fPoint[] weightPoints, int totalSamples = 0, bool mirror = false, bool isAudio = false, fPoint[] sampleValues = null)
        {
            if(sampleValues == null)
            {
                //If the sample values are empty and there is no sample amount specified the bezier will return
                if(totalSamples == 0)
                    return;

                //If the bezier doesn't have a sample value input, it will generate a new set (i.e. calculate the curve)
                samples = new fPoint[totalSamples];

                if (mirror)
                {
                    samples = BigMaths.MirrorBezier(weightPoints, totalSamples, isAudio);
                }
                else
                {
                    samples = BigMaths.CreateBezier(weightPoints, totalSamples);
                }
            }
            else
            {
                samples = new fPoint[sampleValues.Length];

                samples = sampleValues;
            }

            points = new fPoint[weightPoints.Length];
            points = weightPoints;

            numSamples = samples.Length;
            numPoints = points.Length;
        }

        public fPoint GetSample(int sampleIndex)
        {
            if (sampleIndex < 0)
                sampleIndex = 0;
            else if (sampleIndex >= numSamples)
                sampleIndex = numSamples - 1;

            fPoint sample = new fPoint(samples[sampleIndex].x, samples[sampleIndex].y);

            return sample;
        }

        public fPoint[] GetSamples()
        {
            return samples;
        }

        public fPoint GetPoint(int pointIndex)
        {
            fPoint point = points[pointIndex];

            return point;
        }

        public fPoint[] GetPoints()
        {
            return points;
        }

        public int GetNumSamples()
        {
            return numSamples;
        }

        public int GetNumPoints()
        {
            return numPoints;
        }

        //Finds the maximum sample vale from the sample array
        public float GetMaxSample()
        {
            float output = 0.0f;

            for(int i = 0; i < numSamples; i++)
            {
                if (output < samples[i].y)
                    output = samples[i].y;
            }

            return output;
        }

        //Finds the minimum sample value from the sample array
        public float GetMinSample()
        {
            float output = 0.0f;

            for (int i = 0; i < numSamples; i++)
            {
                if (output > samples[i].y)
                    output = samples[i].y;
            }

            return output;
        }

        public float GetRange()
        {
            float output = 0.0f;

            output = samples[numSamples - 1].x - samples[0].x;

            return output;
        }

        public bool ChangePoint(fPoint newPoint, int index)
        {
            if(index <= 0 || index >= numPoints - 1)
            {
                return false;
            }

            points[index] = newPoint;

            samples = BigMaths.CreateBezier(points, numSamples);

            return true;
        }

        //public void SmoothTransition(fPoint[] newPosition, int steps = 50)
        //{
        //    for(int i = 0; i < numSamples; i++)
        //    {
        //        samples[i]
        //    }
        //}

        //Maps the x and y values to work within the bounds of traditional dsp.
        //X is a linear progression and y remains within the bounds of -1 & 1.

        public fPoint[] MapPointsToAudio()
        {
            float min = GetMinSample();
            float max = GetMaxSample();
            float newPoint = 0.0f;
            fPoint[] output = new fPoint[points.Length];

            for(int i = 0; i < numPoints; i++)
            {
                newPoint = BigMaths.Interpolate(-1.0f, 1.0f, BigMaths.Reverpolate(min, max, points[i].y));
                output[i].y = newPoint;
                output[i].x = points[i].x;
            }

            return output;
        }
    }
}
