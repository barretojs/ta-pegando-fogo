using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Geocoding;
using Geocoding.Google;
using SpaceAppsChallenge.Model;
using SpaceAppsChallenge.Analysers;
using System.Net.Http;
using System.Threading;
using SpaceAppsChallenge.CMS.Controllers;

namespace SpaceAppsChallenge.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("NASA International Space Apps Challenge");
            Console.WriteLine("Synchronization Agent\n");

            Console.ForegroundColor = ConsoleColor.White;
            LogStatus("[SCHEDULED]");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                SyncData();
                LogStatus("[SCHEDULED]");
            }


            Console.WriteLine("END");
            Console.Read();
     
        }

        public static void LogStatus(string action, params object[] args)
        {
            Console.WriteLine("[{0}] {1}", DateTime.Now, string.Format(action, args));
        }


        public static void SyncData()
        {
            LogStatus("[DOWNLOADING] MODIS Data information");
            Thread.Sleep(2000);
            LogStatus("[SUCCESS] Download MODIS Data information");

            Console.WriteLine();
            LogStatus("[PARSING] MODIS Data information");
            List<FireIncident> incidents = new List<FireIncident>();
            using (StreamReader reader = new StreamReader(@"Data\MODIS_C6_South_America_7d.csv"))
            {
                reader.ReadLine();
                while (reader.Peek() >= 0)
                {
                    string csvLine = reader.ReadLine();
                    incidents.Add(new FireIncident(csvLine));
                }
            }

            LogStatus("[SUCCESS] Parsing MODIS Data information - Fire Incidents: {0}", incidents.Count);
            Console.WriteLine();

            LogStatus("[LOADING] Receivers");


            

            List<Receiver> receivers = new List<Receiver>();
            receivers.Add(new Receiver("Defesa Civil", "+5518991005519", -9.772f, -36.067f, 20 * 1000)); //KM - Bochexa
            receivers.Add(new Receiver("Corpo de Bombeiros", "+5511998564184", -9.772f, -36.067f, 20 * 1000)); //KM - Landim
            receivers.Add(new Receiver("Representante local",   "+5517991721950",   -40697,     -63131,     10)); //Mt

            

            Console.WriteLine();

            LogStatus("[MATCHING] Fire information");
            List<Report> reports = FireAnalyser.Default.Analyze(incidents, receivers);

            LogStatus("[SENDING] Reports");
            Console.WriteLine();
            foreach (Report report in reports)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                bool reportIsSent = SendReport(report);
                LogStatus("[CALLING] {0} - Fire incidents: {1} - Status: {2}", report.Receiver.Name, report.Incidents.Count, reportIsSent ? "[OK]" : "[ERROR]");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();
            LogStatus("[SUCCESS] Fire report is completed \n\n");
        }

        public static bool SendReport(Report report)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = string.Format("http://pegando-fogo-6u4c.localhost.run/?umidade=13&vento=6&latLng=-20%20-49&usuario=2&confidence=95&tel={0}", report.Receiver.Telephony);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    return response.IsSuccessStatusCode;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
