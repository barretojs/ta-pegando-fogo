using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAppsChallenge.Model
{
    public class Position
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Position(float latitude, float longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

    }
}
