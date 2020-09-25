using System.Collections;
using System.Collections.Generic;
using System;

namespace waves
{
    public static class BigMaths
    {
        public static int C(int collection, int amount)
        {
            int output = 1;
            int numerator = Factorial(collection);
            int denominator = (Factorial(amount) * Factorial(collection - amount));

            if(denominator != 0)
            {
                output = numerator / denominator;
            }

            return output;
        }

        public static int Factorial(int value)
        {
            if (value < 0)
                return 0;

            if (value == 0)
                return 1;

            int output = 1;

            for (int i = 2; i <= value; i++)
            {
                output *= i;
            }

            return output;
        }

        public static fPoint[] MirrorBezier(fPoint[] points, int numSamples, bool isAudio)
        {
            //Creates a normal bezier
            fPoint[] standardBezier = CreateBezier(points, numSamples);

            float range = points[points.Length - 1].x - points[0].x;
            int indexCoef = 2;

            if (isAudio)
            {
                numSamples *= 2;
                indexCoef = 1;
            }

            fPoint[] mirroredBezier = new fPoint[numSamples];

            for (int i = 0; i < numSamples / 2; i++)
            {
                mirroredBezier[i].x = standardBezier[i * indexCoef].x - range;
                mirroredBezier[i].y = standardBezier[i * indexCoef].y * -1;
            }

            for (int i = numSamples / 2; i < numSamples; i++)
            {
                mirroredBezier[i].x = standardBezier[(i - (numSamples / 2)) * indexCoef].x;
                mirroredBezier[i].y = standardBezier[(i - (numSamples / 2)) * indexCoef].y;
            }

            return mirroredBezier;
        }

        public static fPoint[] CreateBezier(fPoint[] points, int numSamples)
        {
            /*
             This Bezier function is for the creation of a single bezier curve.
             It takes an array of fPoints as arguments & the total amount of samples - This may be proportional to the sample rate for audio.

            'p[i].x > p[i+1].x' should always be false. An assertion needs to be implemented to prevent that.
            'p.Length' should always be <= 5.
            */

            //Define amount of points
            int totalPoints = points.Length - 1;
            float prevX = points[0].x;

            fPoint[] outputPoints = new fPoint[numSamples];

            //Loop through each point
            //for (int i = 1; i <= totalPoints; i++)
            //{
            //    //Check that the x is always an increase from the previous.
            //    //Otherwise return null and stop the operation.
            //    if (prevX >= points[i].x)
            //    {
            //        points[i].x = prevX + 0.01f;
            //    }

            //    prevX = points[i].x;
            //}

            float t = 0.0f; //t always ranges between 0 & 1.

            float min = points[0].x; //min point of the curve.

            float max = points[totalPoints].x; //max point of the curve.

            float range = max - min; //total range between the x coordinates of the min and max point.

            float interval = range / numSamples; //the x interval between each sample, in relation to the total number of samples.

            //Loop through for every sample.
            for (int i = 0; i < numSamples; i++)
            {
                //maps the x range to 0 - 1.
                t = Reverpolate(0, numSamples, i);

                //Calls the Bezier Point function to calculate each individual sample.
                outputPoints[i] = BezierPointf(t, points);
            }

            return outputPoints;
        }


        public static fPoint BezierPointf(float t, fPoint[] points)
        {
            int n = points.Length - 1; //This is the polynomial degree, one less than the total amount of points.
            float x = points[0].x; //The output value for the X position.
            float y = points[0].y; //The output value for the y position.
            float bCoeficient = 0.0f; //The coeficient to multiply the incoming coordinates by.

            //This coefficient is just the central part of the bezier equation (nCi * (1-t)^n-i * t).

            //Loops through each integer. Acts as the sigma funciton.
            for (int i = 0; i <= n; i++)
            {
                bCoeficient = (float)(C(n, i) * Math.Pow(1 - t, n - i) * Math.Pow(t, i));

                x += bCoeficient * points[i].x;
                y += bCoeficient * points[i].y;
            }
            
            return new fPoint(x, y);
        }

        public static int Floor(float val)
        {
            int output = 0;
            float remainder = val;

            if (val > 0)
            {
                for (int i = 0; i < val; i++)
                {
                    output = i;
                }
            }
            else if(val < 0)
            {
                for (int i = 0; i > val; i--)
                {
                    output = i;
                }
            }

            return output;
        }

        public static int FactorBounds(int val, bool lowest = false)
        {
            int output = 0;
            int high = val;
            int low = 1;

            for(int i = 2; i < val / 2; i++)
            {
                if(val % i == 0)
                {
                    high = val / i;
                    low = i;

                    break;
                }  
            }

            if(lowest)
            {
                output = low;
            }
            else if(!lowest)
            {
                output = high;
            }

            return output;
        }

        public static float Reverpolate(float min, float max, float value, bool clamp = true)
        {
            float output = 0;

            output = (1 / (max - min)) * value;

            if (clamp)
            {
                if (output > 1)
                    output = 1;
                else if (output < 0)
                    output = 0;
            }

            return output;
        }


        public static float Interpolate(float min, float max, float value, bool clamp = true)
        {
            float output = 0;

            output = min + ((max - min) * value);

            if (clamp)
            {
                if (output > max)
                    output = max;
                else if (output < min)
                    output = min;
            }

            return output;
        }
    }
}
