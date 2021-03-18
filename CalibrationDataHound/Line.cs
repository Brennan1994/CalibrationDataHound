using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHound
{
    public class Line
    {

        // Variables
        private List<Point> _pointsList;

        // Getters
        public List<Point> getPointsList()
        {
            return this._pointsList;
        }

        public Point getPoint(int index)
        {
            return this._pointsList[index];
        }

        public int getVerticesCount()
        {
            return this._pointsList.Count;
        }

        public double getIntegratedArea()
        {
            return this.MidpointIntegral();
        }

        public double[] getXords()
        {
            return this.GetOrdArrayFromLineOrdPairs(0);
        }

        public double[] getYords()
        {
            return this.GetOrdArrayFromLineOrdPairs(1);
        }

        // Constructor
        public Line()
        {
            this._pointsList = new List<Point>();
        }

        public Line(double[] x, double[] y)
        {
            this._pointsList = new List<Point>();
            for (int i = 0; (i < x.Length); i++)
            {
                Point pt = new Point(x[i], y[i]);
                this._pointsList.Add(pt);
            }

        }

        // Methods
        public void RemovePoint(int index)
        {
            this._pointsList.RemoveAt(index);
        }

        public void AddPoint(Point pnt)
        {
            this._pointsList.Add(pnt);
        }

        private double MidpointIntegral()
        {
            // Returns integral of a line using midpoint rectangular integration
            double area = 0;
            for (int i = 0; (i < (_pointsList.Count - 1)); i++)
            {
                double tmpHeight = (_pointsList[i].y + _pointsList[(i + 1)].y) / 2;
                double tmpWidth = (_pointsList[(i + 1)].x - _pointsList[i].x);
                area += tmpHeight * tmpWidth;
            }

            return area;
        }

        private double[] GetOrdArrayFromLineOrdPairs(int xOrYIndex)
        {
            List<Point> pointList = this._pointsList;
            double[] Ords = new double[pointList.Count];
            for (int i = 0; (i < pointList.Count); i++)
            {
                if ((xOrYIndex == 0))
                {
                    Ords[i] = pointList[i].x;
                }
                else
                {
                    Ords[i] = pointList[i].y;
                }

            }

            return Ords;
        }
    }
}
