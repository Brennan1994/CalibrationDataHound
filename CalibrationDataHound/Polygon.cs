using System;
using System.Collections.Generic;

namespace CalibrationDataHound;

public class Polygon
{
    public List<Point> Vertices { get; }

    public Polygon(double[] x, double[] y)
    {
        for (int i = 0; (i < x.Length); i++)
        {
            Point pt = new (x[i], y[i]);
            Vertices.Add(pt);
        }

    }

    public static float AreaOfTriangle(Point point0, Point point1, Point point2)
    {
        // taken from Will Lehman's area function in LifeSimGIS by the same name. Takes 3 points and returns the area of the triangle formed by them.
        return ((float)(Math.Abs((((point0.X * point1.Y)
                        + ((point1.X * point2.Y)
                        + ((point2.X * point0.Y)
                        - ((point1.X * point0.Y)
                        - ((point2.X * point1.Y)
                        - (point0.X * point2.Y))))))
                        * 0.5))));
    }
}

