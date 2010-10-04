using System;

namespace SensorShare
{
    /// <summary>
    /// Data from each of the four sensors
    /// </summary>
    public class ReadingsRequest
    {
        public DateTime Time
        {
            get { return DateTime.FromFileTimeUtc(TimeBinary); }
            set { TimeBinary = value.ToFileTimeUtc(); }
        }
        
        public long TimeBinary;

        public int Minutes;

        /// <summary>
        /// Message to request data from server, with start time and timespan in minutes
        /// </summary>
       /// <param name="time">Time to get readings for</param>
       /// <param name="mintesBefore">Minutes before time to send readings from</param>
        public ReadingsRequest(DateTime startTime, int mintesBefore)
        {

            this.Time = startTime;
            this.Minutes = mintesBefore;
        }

        public ReadingsRequest()
        {
        }
    }
}