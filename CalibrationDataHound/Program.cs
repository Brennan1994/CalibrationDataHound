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
            string USGSDownloadURL = "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=0";
            props.read(propertiesFileName);
            bool dataExists;

            foreach (Gage gage in props.MyGages)
            {
                gage.DownloadRatingCurve(USGSDownloadURL+gage.gageNumber.ToString(),currentDirectory+"\\"+gage.gageNumber.ToString()+".txt");
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
