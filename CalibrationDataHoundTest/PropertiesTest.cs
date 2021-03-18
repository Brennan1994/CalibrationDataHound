using NUnit.Framework;
using CalibrationDataHound;

namespace CalibrationDataHoundTest
{
    class PropertiesTest
    {
        [Test]
        public void PropertiesReadReadsProperties()
        {
            Properties prop = new Properties();
            prop.read("C:\\Programs\\Git\\Repos\\CalibrationDataHound\\Properties.txt");
        }
    }
}
