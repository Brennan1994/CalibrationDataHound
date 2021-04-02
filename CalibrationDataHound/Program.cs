using System;
using System.IO;
namespace CalibrationDataHound
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            Properties props = new Properties();
            string currentDirectory = Directory.GetCurrentDirectory();
            string propertiesFileName = currentDirectory + "\\properties.txt";
            string USGSSiteInformationDownloadURL = "https://waterservices.usgs.gov/nwis/site/?format=rdb&sites=01646500&siteStatus=all";
            props.read(propertiesFileName);
            bool dataExists;

            foreach (Gage gage in props.MyGages)
            {
                gage.DownloadRatingCurve(gage.USGSRatingCurveURL,currentDirectory+"\\"+gage.gageNumber.ToString()+".txt");
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
