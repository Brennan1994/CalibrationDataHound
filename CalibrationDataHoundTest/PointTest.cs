using CalibrationDataHound;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHoundTest
{
    class PointTest
    {
        readonly Point pt0_0 = new(0, 0);
        readonly Point pt1_1 = new(1, 1);
        readonly Point pt2_4 = new(2, 4);
        readonly Point pt100_0 = new(100, 0);
        readonly Point pt50_neg100 = new(50, -100);

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
