using System.Collections;
using ScienceScope;

namespace SensorShare
{
    public class SensorDescriptionsData : CollectionBase
    {
        public void Add(SensorDefinition sensor)
        {
            List.Add(sensor);
        }

        public void Remove(SensorDefinition sensor)
        {
            List.Remove(sensor);
        }

        public SensorDefinition this[int sensorIndex]
        {
            get
            {
                return (SensorDefinition)List[sensorIndex];
            }
            set
            {
                List[sensorIndex] = value;
            }
        }

        public bool NoneConnected()
        {
           bool notConnected = true;
           foreach (SensorDefinition sensor in this)
           {
              if ((sensor.ID != 255) || (sensor.Range != 255))
              {
                 notConnected = false;
              }
           }
           return notConnected;
        }


    }
}