using System.Collections;
using System.Collections.Generic;

namespace waves
{
    public class Datatypes
    {

    }

    public struct iPoint
    {
        public int x { get; set; }
        public int y { get; set; }

        public iPoint(int X, int Y)
        {
            x = X;
            y = Y;    
        }
    }

    public struct fPoint
    {
        public float x { get; set; }
        public float y { get; set; }

        public fPoint(float X, float Y)
        {
            x = X;
            y = Y;
        }
    }
}
