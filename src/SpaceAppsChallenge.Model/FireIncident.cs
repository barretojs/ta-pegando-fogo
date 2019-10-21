using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAppsChallenge.Model
{
    public class FireIncident
    {
        public Position Position { get; set; }
        public DateTime AcquiredDate { get; set; }
        public int ConfidenceLevel { get; set; }
        public WeatherData WeatherData { get; set; }
        public FireIncident(float latitude, float longitude, DateTime acquiredDate, int confidenceLevel)
        {
            this.Position = new Position(latitude, longitude);
            this.AcquiredDate = acquiredDate;
            this.ConfidenceLevel = confidenceLevel;
        }

        public FireIncident(string csvLine)
        {
            string[] data = csvLine.Split(',');

            float latitude = float.Parse(data[0], CultureInfo.InvariantCulture);
            float longitude = float.Parse(data[1], CultureInfo.InvariantCulture);

            this.Position = new Position(latitude, longitude);
            this.AcquiredDate = DateTime.Parse(data[5], CultureInfo.InvariantCulture);
            this.ConfidenceLevel = int.Parse(data[8], CultureInfo.InvariantCulture);
        }
    }
}
