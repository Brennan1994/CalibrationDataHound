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
        public bool isNAVD88 { get; set; }
        public Point Location { get; set; }
        public string USGSRatingCurveURL
        {
            get
            {
                return "https://waterdata.usgs.gov/nwisweb/get_ratings?file_type=exsa&site_no=0" + gageNumber;
            }
        }
        public string USGSSiteInformationDownloadURL
        {
            get
            {
                return "https://waterservices.usgs.gov/nwis/site/?format=rdb&sites=0" + gageNumber + "&siteStatus=all";
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

        public void DownloadSiteInformation(string USGS_url_string, string filePathToSave)
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
                    this.datum = Double.Parse(cutLine[datumIndex]);
                    double lat = Double.Parse(cutLine[LatIndex]);
                    double lon = Double.Parse(cutLine[LonIndex]);
                    this.Location = new Point(lat, lon);
                    if (cutLine[datumTypeIndex].Contains("NAVD88")) { this.isNAVD88 = true; }
                    else { this.isNAVD88 = false; }
                }
            }
            return true;
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
