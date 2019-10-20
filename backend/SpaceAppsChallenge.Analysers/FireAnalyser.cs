using SpaceAppsChallenge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace SpaceAppsChallenge.Analysers
{
    public class FireAnalyser
    {
        public static FireAnalyser Default
        {
            get
            {
                return new FireAnalyser();
            }
        }

        public List<Report> Analyze(List<FireIncident> incidents, List<Receiver> receivers) {

            List<Report> reports = new List<Report>();
            foreach (Receiver receiver in receivers) 
            {
                Report newReport = new Report(receiver);

                foreach (FireIncident incident in incidents)
                {
                    double distanceFromFire = this.GetDistanceInMeters(
                        incident.Position.Latitude, 
                        incident.Position.Longitude, 
                        receiver.Position.Latitude, 
                        receiver.Position.Longitude);

                    if (distanceFromFire <= receiver.OperatingRange)
                    {
                        //incident.WeatherData = this.GetWeatherDataFromFireIncident(incident);
                        newReport.Incidents.Add(incident);
                    }
                }

                if (newReport.Incidents.Any() == true)
                {
                    reports.Add(newReport);
                }
            }

            return reports;
        }

        public WeatherData GetWeatherDataFromFireIncident(FireIncident incident)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = string.Format(
                        "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid=f48d93cf79caeb4cdf990386aa8df5f3&units=metric", 
                        incident.Position.Latitude,
                        incident.Position.Longitude);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    string json = response.Content.ReadAsStringAsync().Result;
                    return new WeatherData();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(DegreeToRadians(lat1)) * Math.Sin(DegreeToRadians(lat2)) + Math.Cos(DegreeToRadians(lat1)) * Math.Cos(DegreeToRadians(lat2)) * Math.Cos(DegreeToRadians(theta));
                dist = Math.Acos(dist);
                dist = RadiansToDegree(dist);
                dist = dist * 60 * 1.1515;

                double distanceInKilometers = dist * 1.609344;
                return distanceInKilometers * 1000;
            }
        }

        public static double DegreeToRadians(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        public static double RadiansToDegree(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

    }
}
