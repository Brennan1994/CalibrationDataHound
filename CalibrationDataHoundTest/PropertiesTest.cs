using NUnit.Framework;
using CalibrationDataHound;
using System.IO;

namespace CalibrationDataHoundTest
{
    class PropertiesTest
    {
        [Test]
        public void PropertiesReadReadsProperties()
        {
            Properties prop = new();
            string relativePathToPropsFile = @"..\..\..\..\Properties.txt";
            prop.read(relativePathToPropsFile);
        }
    }
}
