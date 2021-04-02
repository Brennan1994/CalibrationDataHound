using CalibrationDataHound;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalibrationDataHoundTest
{
    class ProgramTest
    {
        [Test]
        public void ProgramHasNoRuntimeErrors()
        {
            Console.WriteLine("Begin");
            Properties props = new Properties();
            string currentDirectory = Directory.GetCurrentDirectory();
            string propertiesFileName = currentDirectory + "\\properties.txt";
            string USGSRatingCurveDownloadURL = "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=0";
            props.read(propertiesFileName);
            bool dataExists;

            foreach (Gage gage in props.MyGages)
            {
                gage.DownloadRatingCurve(USGSRatingCurveDownloadURL + gage.gageNumber.ToString(), currentDirectory + "\\" + gage.gageNumber.ToString() + ".txt");
                dataExists = gage.ParseRatingCurveTextfile(currentDirectory + "\\" + gage.gageNumber.ToString() + ".txt");
                if (dataExists)
                {
                    gage.ratingCurveRaw = LineThinner.DouglasPeukerReduction(gage.ratingCurveRaw, .01);
                    gage.ConvertToNAVD88();
                    gage.WriteToCSV(currentDirectory + "\\" + gage.gageNumber.ToString() + ".csv");
                }
                else
                {
                    //do nothing
                }
            }
            Console.WriteLine("Done");
        }
    }
}
