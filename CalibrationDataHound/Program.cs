using System;
using System.IO;
namespace CalibrationDataHound
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            Properties props = new();
            string currentDirectory = Directory.GetCurrentDirectory();
            string propertiesFileName = currentDirectory + "\\properties.txt";
            props.read(propertiesFileName);
            bool dataExists;

            foreach (Gage gage in props.MyGages)
            {
                Gage.DownloadRatingCurve(gage.USGSRatingCurveURL,currentDirectory+"\\"+gage.GageNumber.ToString()+".txt");
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
