using CsvHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace CalibrationDataHound
{
    public class Gage
    {
        public double datum { get; set; }
        public int gageNumber { get; set; }
        public string name { get; set; }
        public double conversionToNAVD88 { get; set; }
        public Line ratingCurveRaw { get; set; }
        public Line ratingCurve { get; set; }
        public string USGSRatingCurveURL
        {
            get
            {
                return "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=" + gageNumber;
            }
        }



        public Gage (string name, int gageNumber, float datum, float conversionToNAVD88)
        {
            this.name = name;
            this.gageNumber = gageNumber;
            this.datum = datum;
            this.conversionToNAVD88 = conversionToNAVD88;
        }

        public void DownloadRatingCurve(string USGS_url_string, string filePathToSave)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(USGS_url_string, filePathToSave);
            }
        }

        public bool ParseRatingCurveTextfile(string filepath)
        {
            this.ratingCurveRaw = new Line();
            string[] lines = System.IO.File.ReadAllLines(filepath);
            string seperator = "\t";
            foreach(string line in lines)
            {
                string[] splitLine = line.Split(seperator);
                if (double.TryParse(splitLine[0],out var number))
                {
                    double stage = number;
                    double flow = double.Parse(splitLine[2]);
                    Point myPoint = new Point(stage, flow);
                    ratingCurveRaw.AddPoint(myPoint);
                }
            }
            if (ratingCurveRaw.getVerticesCount()<1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ConvertToNAVD88()
        {
            this.ratingCurve = new Line();
            foreach(Point point in ratingCurveRaw.getPointsList())
            {
                double convertedStage = point.x + datum + conversionToNAVD88;
                Point myPoint = new Point(convertedStage, point.y);
                ratingCurve.AddPoint(myPoint);
            }
        }

        public void thinRatingCurve()
        {
            ratingCurve = LineThinner.DouglasPeukerReduction(ratingCurve, .1);
        }


        public void WriteToCSV(string filepath)
        {

            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            foreach (var record in ratingCurve.getPointsList())
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}
