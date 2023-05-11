using System;

namespace CalibrationDataHound
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Calculates the distance from myPoint to the line defined by lineStart and lineEnd
        public static double PerpendicularDistance(Point lineStart, Point lineEnd, Point myPoint)
        {
            double area = Math.Abs(0.5 * (lineStart.X * (lineEnd.Y - myPoint.Y) + lineEnd.X * (myPoint.Y - lineStart.Y) + myPoint.X * (lineStart.Y - lineEnd.Y)));
            double Base = Math.Sqrt(Math.Pow(lineStart.X - lineEnd.X, 2) + Math.Pow(lineStart.Y - lineEnd.Y, 2));
            return (area / Base * 2); //height
            // height
        }
    }
}
