using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHound
{
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }

        // constructor
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Calculates the distance from myPoint to the line defined by lineStart and lineEnd
        public static double PerpendicularDistance(Point lineStart, Point lineEnd, Point myPoint)
        {
            double area = Math.Abs(0.5 * (lineStart.x * (lineEnd.y - myPoint.y) + lineEnd.x * (myPoint.y - lineStart.y) + myPoint.x * (lineStart.y - lineEnd.y)));
            double Base = Math.Sqrt(Math.Pow(lineStart.x - lineEnd.x, 2) + Math.Pow(lineStart.y - lineEnd.y, 2));
            return (area / Base * 2); //height
            // height
        }
        // Adjusts x
        private void adjustX(double increment)
        {
            x += increment;
        }
    }
}
