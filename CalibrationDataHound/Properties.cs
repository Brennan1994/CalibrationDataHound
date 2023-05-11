using System.Collections.Generic;

namespace CalibrationDataHound
{
    public class Properties
    {
        public List<Gage> MyGages = new();
        
        public void read(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            string seperator = ",";
            foreach (string line in lines)
            {
                string[] splitLine = line.Split(seperator);
                Gage MyGage = new(splitLine[1], int.Parse(splitLine[0]), float.Parse(splitLine[2]), float.Parse(splitLine[3]));
                MyGages.Add(MyGage);
            }
        }
    }
}
 
