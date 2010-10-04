using System;

namespace ScienceScope
{
    public class SensorID
    {
        public int ID = 255;
        public int Range = 255;
    }

    public class SensorDefinition : SensorID
    {
        private string name = null;
        public string Name
        {
            get
            {
                if (name == null)
                    return "Not connected";
                else
                    return name;
            }
            set { name = value; }
        }

        public string ASCIIUnit = "";
        public string Unit = "";
        public double MaxValue = 0;
        public double MinValue = 100;

        public string Description
        {
            get
            {
                if (ID == 255)
                {
                    return "Not Connected";
                }
                else
                {
                    string returnString = "";

                    if (Unit != null)
                    {
                        if (Name != null)
                        {
                            returnString = String.Format("{0} in {1}", Name, Unit);
                        }
                        else
                        {
                            returnString = String.Format("Reading in {0}", Unit);
                        }
                    }
                    else
                    {
                        returnString = "Unknown";
                    }
                    return returnString;
                }
            }
        }
    }
}