using CalibrationDataHound;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHoundTest
{
    class PointTest
    {

        Point pt0_0 = new Point(0, 0);

        Point pt1_1 = new Point(1, 1);

        Point pt2_4 = new Point(2, 4);

        Point pt100_0 = new Point(100, 0);

        Point pt50_neg100 = new Point(50, -100);

        [Test()]
        public void perpendicularDistanceShouldCalcCorrectOnSlopeLine()
        {
            Assert.IsTrue((Math.Abs((Point.PerpendicularDistance(this.pt0_0, this.pt2_4, this.pt1_1) - 0.44721359549995793)) < 0.0001));
        }

        [Test()]
        public void perpendicularDistanceShouldCalcCorrectOnHorizontalLine()
        {
            Assert.AreEqual(Point.PerpendicularDistance(this.pt0_0, this.pt100_0, this.pt1_1), 1);
        }

        [Test()]
        public void perpendicularDistanceShouldCalcCorrectInNegativeSpace()
        {
            Assert.AreEqual(Point.PerpendicularDistance(this.pt0_0, this.pt100_0, this.pt50_neg100), 100);
        }
    }
}
