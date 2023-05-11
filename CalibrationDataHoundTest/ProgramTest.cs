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
            Properties props = new();
            string currentDirectory = Directory.GetCurrentDirectory();
            string propertiesFileName = @"..\..\..\..\PropertiesGreenbrier.txt";
            string USGSRatingCurveDownloadURL = "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=0";
            props.read(propertiesFileName);
            bool dataExists;

            foreach (Gage gage in props.MyGages)
            {
                Gage.DownloadRatingCurve(USGSRatingCurveDownloadURL + gage.GageNumber.ToString(), currentDirectory + "\\" + gage.GageNumber.ToString() + ".txt");
                dataExists = gage.ParseRatingCurveTextfile(currentDirectory + "\\" + gage.GageNumber.ToString() + ".txt");
                if (dataExists)
                {
                    gage.RatingCurveRaw = LineThinner.DouglasPeukerReduction(gage.RatingCurveRaw, .01);
                    gage.ConvertToNAVD88();
                    gage.WriteToCSV(currentDirectory + "\\" + gage.GageNumber.ToString() + ".csv");
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
