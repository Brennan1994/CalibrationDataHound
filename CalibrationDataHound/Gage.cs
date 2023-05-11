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
        public double Datum { get; set; }
        public int GageNumber { get; set; }
        public string Name { get; set; }
        public double ConversionToNAVD88 { get; set; }
        public Line RatingCurveRaw { get; set; }
        public Line RatingCurve { get; set; }
        public bool IsNAVD88 { get; set; }
        public Point Location { get; set; }
        public string USGSRatingCurveURL
        {
            get
            {
                return "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=0" + GageNumber;
            }
        }
        public string USGSSiteInformationDownloadURL
        {
            get
            {
                return "https://waterservices.usgs.gov/nwis/site/?format=rdb&sites=0" + GageNumber + "&siteStatus=all";
            }
        }



        public Gage (string name, int gageNumber, float datum, float conversionToNAVD88)
        {
            Name = name;
            GageNumber = gageNumber;
            Datum = datum;
            ConversionToNAVD88 = conversionToNAVD88;
        }

        public static void DownloadRatingCurve(string USGS_url_string, string filePathToSave)
        {
            using var client = new WebClient();
            client.DownloadFile(USGS_url_string, filePathToSave);
        }

        public static void DownloadSiteInformation(string USGS_url_string, string filePathToSave)
        {
            using var client = new WebClient();
            client.DownloadFile(USGS_url_string, filePathToSave);
        }

        public bool ParseRatingCurveTextfile(string filepath)
        {
            RatingCurveRaw = new Line();
            string[] lines = File.ReadAllLines(filepath);
            string seperator = "\t";
            foreach(string line in lines)
            {
                string[] splitLine = line.Split(seperator);
                if (double.TryParse(splitLine[0],out var number))
                {
                    double stage = number;
                    double flow = double.Parse(splitLine[2]);
                    Point myPoint = new(stage, flow);
                    RatingCurveRaw.AddPoint(myPoint);
                }
            }
            if (RatingCurveRaw.PointsList.Count<1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ParseGageInfoTextfile(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            string seperator = "\t";
            int headerIndex = int.MaxValue;
            int datumIndex = int.MaxValue;
            int datumTypeIndex = int.MaxValue;
            int LatIndex = int.MaxValue;
            int LonIndex = int.MaxValue;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains('#'))
                {
                    if (!lines[i + 1].Contains('#'))
                    {
                        headerIndex = i + 1;
                    }
                    
                }
                if (i == headerIndex)
                {
                    string[] splitHeaders = lines[i].Split(seperator);
                    for (int ii = 0; ii < splitHeaders.Length; ii++)
                    {
                        if (splitHeaders[ii] == "dec_long_va") { LonIndex = ii; }
                        if (splitHeaders[ii] == "dec_lat_va") { LatIndex = ii; }
                        if (splitHeaders[ii] == "alt_va") { datumIndex = ii; }
                        if (splitHeaders[ii] == "alt_datum_cd") { datumTypeIndex = ii; }
                    }
                }
                if (i == headerIndex + 1) { continue; }
                else
                {
                    string[] cutLine = lines[i].Split(seperator);
                    Datum = double.Parse(cutLine[datumIndex]);
                    double lat = double.Parse(cutLine[LatIndex]);
                    double lon = double.Parse(cutLine[LonIndex]);
                    Location = new Point(lat, lon);
                    if (cutLine[datumTypeIndex].Contains("NAVD88")) { IsNAVD88 = true; }
                    else { IsNAVD88 = false; }
                }
            }
            return true;
        }


        public void ConvertToNAVD88()
        {
            RatingCurve = new Line();
            foreach(Point point in RatingCurveRaw.PointsList)
            {
                double convertedStage = point.X + Datum + ConversionToNAVD88;
                Point myPoint = new(convertedStage, point.Y);
                RatingCurve.AddPoint(myPoint);
            }
        }

        public void ThinRatingCurve()
        {
            RatingCurve = LineThinner.DouglasPeukerReduction(RatingCurve, .1);
        }


        public void WriteToCSV(string filepath)
        {

            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            foreach (var record in RatingCurve.PointsList)
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}
