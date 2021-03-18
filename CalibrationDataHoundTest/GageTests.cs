using NUnit.Framework;
using CalibrationDataHound;
using System.IO;

namespace CalibrationDataHoundTest
{
    public class Gage_Tests
    {
        private static string name = "Sabana Rv nr De Leon, TX";
        private static int gagenumber = 08099300;
        private static float datum = 234;
        private static float convert2NavD88 = .04f;
        private static string downloadFilePath = "C:\\Temp\\" + gagenumber.ToString() + ".csv";
        private static string USGSUrl = "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=08099300";
        Gage testGage = new Gage(name, gagenumber, datum, convert2NavD88);
        
        [Test]
        public void DownloadRatingCurveReturnsRatingCurve()
        {
            testGage.DownloadRatingCurve(USGSUrl, downloadFilePath);
            Assert.IsTrue(File.Exists(downloadFilePath));
        }

        [Test]
        public void ParseRatingCurveReturnsRatingCurve()
        {
            testGage.ParseRatingCurveTextfile(downloadFilePath);
            Assert.IsTrue(testGage.ratingCurveRaw.getVerticesCount() > 0);
        }
        [Test]
        public void WriteToCSVcreatesACSV()
        {
            testGage.ParseRatingCurveTextfile(downloadFilePath);
            testGage.ConvertToNAVD88();
            testGage.WriteToCSV("C:\\Temp\\test.csv");
            Assert.IsTrue(File.Exists("C:\\Temp\\test.csv"));
        }
    }
}