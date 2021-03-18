using CalibrationDataHound;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationDataHoundTest
{
    class LineThinnerTest
    {
        double[] Xords =
                {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
            29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53,
            54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78,
            79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99};
        //from 0^2 to 99^2
        double[] Yords = {0, 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324, 361, 400, 441,
            484, 529, 576, 625, 676, 729, 784, 841, 900, 961, 1024, 1089, 1156, 1225, 1296, 1369, 1444, 1521, 1600,
            1681, 1764, 1849, 1936, 2025, 2116, 2209, 2304, 2401, 2500, 2601, 2704, 2809, 2916, 3025, 3136, 3249, 3364,
            3481, 3600, 3721, 3844, 3969, 4096, 4225, 4356, 4489, 4624, 4761, 4900, 5041, 5184, 5329, 5476, 5625, 5776,
            5929, 6084, 6241, 6400, 6561, 6724, 6889, 7056, 7225, 7396, 7569, 7744, 7921, 8100, 8281, 8464, 8649, 8836,
            9025, 9216, 9409, 9604, 9801};

        Line yIsXsquared;

        [SetUp]
        public void Setup()
        {
            yIsXsquared = new Line(Xords, Yords);
        }
        [Test]
        public void visvaligamWhyattSimplifyShouldKeepSpecifiedNumPoints()
        {
            Assert.AreEqual(LineThinner.VisvaligamWhyattSimplify(50, yIsXsquared).getVerticesCount(), 50);
        }
        [Test]
        public void visvaligamWhyattSimplifyShouldHaveSimilarAreaUnderCurve()
        {
            var SimpleLineIntegral = LineThinner.VisvaligamWhyattSimplify(50, yIsXsquared).getIntegratedArea();
            Assert.IsTrue(Math.Abs(SimpleLineIntegral - 323433) < .1 * 323433);
            //comparison number is true integral of y=x^2 from 0 to 99
        }
        [Test]
        public void douglasPeukerReductionShouldHaveSimilarAreaUnderCurve()
        {
            Assert.IsTrue(Math.Abs((LineThinner.DouglasPeukerReduction(yIsXsquared, .1).getIntegratedArea()) - 323433) < .01 * 323433);
        }
        [Test]
        public void douglasPeukerReductionShouldSaveFirstPoint()
        {
            Assert.AreEqual(yIsXsquared.getPoint(0), LineThinner.DouglasPeukerReduction(yIsXsquared, .1).getPoint(0));
        }
        [Test]
        public void douglasPeukerReductionShouldSaveLastPoint()
        {
            Assert.AreEqual(yIsXsquared.getPoint(yIsXsquared.getVerticesCount() - 1), LineThinner.DouglasPeukerReduction(yIsXsquared, .1).getPoint(27));
        }
    }
}
