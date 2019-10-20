using SpaceAppsChallenge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaceAppsChallenge.CMS.ViewModels
{
    public class ReceiverViewModel
    {

        public string Name { get; set; }
        public string Telephony { get; set; }
        public int Type { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float OperatingRange { get; set; }
        public string ApiKey { get; set; }

        public ReceiverViewModel()
        {

        }

        public ReceiverViewModel(Receiver receiver)
        {
            this.Name = receiver.Name;
            this.Telephony = receiver.Telephony;
            this.Type = receiver.Type;
            this.Latitude = receiver.Position.Latitude;
            this.Longitude = receiver.Position.Longitude;
            this.OperatingRange = receiver.OperatingRange;
                 
        }

        public Receiver ToReceiver()
        {
            Receiver result = new Receiver(this.Name, this.Telephony, this.Latitude, this.Longitude, this.OperatingRange);
            return result;
        }
    }
}