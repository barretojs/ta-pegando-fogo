using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAppsChallenge.Model
{
    public class Report
    {
        public Receiver Receiver { get; set; }

        public List<FireIncident> Incidents { get; set; }

        public Report(Receiver receiver)
        {
            this.Receiver = receiver;
            this.Incidents = new List<FireIncident>();
        }

    }
}
