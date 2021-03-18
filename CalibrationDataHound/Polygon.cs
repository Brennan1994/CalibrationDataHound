using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHound
{
    public class Polygon
    {

        // fields
        private List<Point> _vertices;

        // getters
        public List<Point> getVertices()
        {
            return this._vertices;
        }

        // constructors
        public Polygon(double[] x, double[] y)
        {
            for (int i = 0; (i < x.Length); i++)
            {
                Point pt = new Point(x[i], y[i]);
                this._vertices.Add(pt);
            }

        }

        // methods
        public static float AreaOfTriangle(Point point0, Point point1, Point point2)
        {
            // taken from Will Lehman's area function in LifeSimGIS by the same name. Takes 3 points and returns the area of the triangle formed by them. *****FIX
            return ((float)(Math.Abs((((point0.x * point1.y)
                            + ((point1.x * point2.y)
                            + ((point2.x * point0.y)
                            - ((point1.x * point0.y)
                            - ((point2.x * point1.y)
                            - (point0.x * point2.y))))))
                            * 0.5))));
        }
    }
}
