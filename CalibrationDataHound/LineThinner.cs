using System;
using System.Collections.Generic;

namespace CalibrationDataHound;

public class LineThinner
{
    private static List<int> reducedLineIndices;

    public static Line DouglasPeukerReduction(Line myLine, double tolerance)
    {
        reducedLineIndices = new List<int>();
        int firstPnt = 0;
        int lastPnt = (myLine.PointsList.Count - 1);
        Line newLine = new();
        reducedLineIndices.Add(firstPnt);
        reducedLineIndices.Add(lastPnt);
        DouglasPeukerReductionIterator(firstPnt, lastPnt, tolerance, myLine);
        reducedLineIndices.Sort();
        foreach (int index in reducedLineIndices)
        {
            Point pointToAdd = myLine.GetPoint(index);
            newLine.AddPoint(pointToAdd);
        }
        return newLine;
    }

    private static void DouglasPeukerReductionIterator(int firstPnt, int lastPnt, double tolerance, Line myLine)
    {
        double maxDistance = 0;
        int farthestIndex = 0;
        double distance;
        for (int i = firstPnt; (i
                    < (lastPnt - 1)); i++)
        {
            distance = Point.PerpendicularDistance(myLine.GetPoint(firstPnt), myLine.GetPoint(lastPnt), myLine.GetPoint(i));
            if ((distance > maxDistance))
            {
                maxDistance = distance;
                farthestIndex = i;
            }

        }

        if (((maxDistance > tolerance)
                    & (farthestIndex != 0)))
        {
            reducedLineIndices.Add(farthestIndex);
            DouglasPeukerReductionIterator(firstPnt, farthestIndex, tolerance, myLine);
            DouglasPeukerReductionIterator(farthestIndex, lastPnt, tolerance, myLine);
        }

    }
    public static Line VisvaligamWhyattSimplify(int numToKeep, Line myLine)
    {
        //Thins a line down to the specified amount of points based on the VW algorithm.
        //Implementation based on the description of the method here: http://bost.ocks.org/mike/simplify/

        int removeLimit = (myLine.PointsList.Count) - numToKeep;
        int minIndex = 1;
        for (int i = 0; i < removeLimit; i++)
        {
            float minArea = Polygon.AreaOfTriangle(myLine.GetPoint(0), myLine.GetPoint(1), myLine.GetPoint(2)); // This is the baseline we'll start our first comparison to.
            for (int j = 2; j < myLine.PointsList.Count - 2; j++) // starting at 2, because we're gonna start calculating areas, and we already calculated area 1.
            { //ending at minus 2 because we don't want the last point, and the size property uses counting numbers, so we have to reduce by an extra -1 to account for 0 index
                float tmpArea = Polygon.AreaOfTriangle(myLine.GetPoint(j - 1), myLine.GetPoint(j), myLine.GetPoint(j + 1));
                if (tmpArea < minArea)
                {
                    minIndex = j;
                    minArea = tmpArea;
                }
                if (minIndex == 875 && j == myLine.PointsList.Count - 3)
                {
                    Console.WriteLine("stop!");
                }
            }
            myLine.RemovePoint(minIndex);
        }
        return myLine;
    }
}
