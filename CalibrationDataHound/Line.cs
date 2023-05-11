using System.Collections.Generic;

namespace CalibrationDataHound;

public class Line
{
    public List<Point> PointsList { get; set; }

    public Point GetPoint(int index)
    {
        return PointsList[index];
    }

    public double[] GetXords()
    {
        return GetOrdArrayFromLineOrdPairs(0);
    }

    public double[] GetYords()
    {
        return GetOrdArrayFromLineOrdPairs(1);
    }

    #region Constructors
    public Line()
    {
        PointsList = new List<Point>();
    }

    public Line(double[] x, double[] y)
    {
        PointsList = new List<Point>();
        for (int i = 0; (i < x.Length); i++)
        {
            Point pt = new(x[i], y[i]);
            PointsList.Add(pt);
        }

    }
    #endregion

    #region Methods
    public void RemovePoint(int index)
    {
        PointsList.RemoveAt(index);
    }
    public void AddPoint(Point pnt)
    {
        PointsList.Add(pnt);
    }
    public double GetMidpointIntegral()
    {
        // Returns integral of a line using midpoint rectangular integration
        double area = 0;
        for (int i = 0; (i < (PointsList.Count - 1)); i++)
        {
            double tmpHeight = (PointsList[i].Y + PointsList[(i + 1)].Y) / 2;
            double tmpWidth = (PointsList[(i + 1)].X - PointsList[i].X);
            area += tmpHeight * tmpWidth;
        }

        return area;
    }
    private double[] GetOrdArrayFromLineOrdPairs(int xOrYIndex)
    {
        List<Point> pointList = PointsList;
        double[] Ords = new double[pointList.Count];
        for (int i = 0; (i < pointList.Count); i++)
        {
            if ((xOrYIndex == 0))
            {
                Ords[i] = pointList[i].X;
            }
            else
            {
                Ords[i] = pointList[i].Y;
            }

        }
        return Ords;
    }
    #endregion
}
