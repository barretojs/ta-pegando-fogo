using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAppsChallenge.Model
{
    public class Receiver
    {
        public string Name { get; set; }
        public string Telephony { get; set; }
        public int Type { get; set; }
        public Position Position { get; set; }
        public float OperatingRange { get; set; }

        public Receiver(string name, string telephony, float latitude, float longitude, float operationRange)
        {
            this.Name = name;
            this.Telephony = telephony;
            this.Position = new Position(latitude, longitude);
            this.OperatingRange = operationRange;
        }

    }
}
