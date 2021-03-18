using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHound
{
    public class LineThinner
    {

        // fields
        private static List<int> reducedLineIndices;

        // methods
        public static Line DouglasPeukerReduction(Line myLine, double tolerance)
        {
            reducedLineIndices = new List<int>();
            int firstPnt = 0;
            int lastPnt = (myLine.getVerticesCount() - 1);
            Line newLine = new Line();
            reducedLineIndices.Add(firstPnt);
            reducedLineIndices.Add(lastPnt);
            LineThinner.DouglasPeukerReductionIterator(firstPnt, lastPnt, tolerance, myLine);
            reducedLineIndices.Sort();
            foreach (int index in reducedLineIndices)
            {
                Point pointToAdd = myLine.getPoint(index);
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
                distance = Point.PerpendicularDistance(myLine.getPoint(firstPnt), myLine.getPoint(lastPnt), myLine.getPoint(i));
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
                LineThinner.DouglasPeukerReductionIterator(firstPnt, farthestIndex, tolerance, myLine);
                LineThinner.DouglasPeukerReductionIterator(farthestIndex, lastPnt, tolerance, myLine);
            }

        }
        public static Line VisvaligamWhyattSimplify(int numToKeep, Line myLine)
        {
            //Thins a line down to the specified amount of points based on the VW algorithm.
            //Implementation based on the description of the method here: http://bost.ocks.org/mike/simplify/

            int removeLimit = (myLine.getVerticesCount()) - numToKeep;
            int minIndex = 1;
            for (int i = 0; i < removeLimit; i++)
            {
                float minArea = Polygon.AreaOfTriangle(myLine.getPoint(0), myLine.getPoint(1), myLine.getPoint(2)); // This is the baseline we'll start our first comparison to.
                for (int j = 2; j < myLine.getVerticesCount() - 2; j++) // starting at 2, because we're gonna start calculating areas, and we already calculated area 1.
                { //ending at minus 2 because we don't want the last point, and the size property uses counting numbers, so we have to reduce by an extra -1 to account for 0 index
                    float tmpArea = Polygon.AreaOfTriangle(myLine.getPoint(j - 1), myLine.getPoint(j), myLine.getPoint(j + 1));
                    if (tmpArea < minArea)
                    {
                        minIndex = j;
                        minArea = tmpArea;
                    }
                    if (minIndex == 875 && j == myLine.getVerticesCount() - 3)
                    {
                        Console.WriteLine("stop!");
                    }
                }
                myLine.RemovePoint(minIndex);
            }
            return myLine;
        }
    }
}
