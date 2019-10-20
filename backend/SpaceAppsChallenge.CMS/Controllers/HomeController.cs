using SpaceAppsChallenge.CMS.ViewModels;
using SpaceAppsChallenge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;

namespace SpaceAppsChallenge.CMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Receivers()
        {
            ViewBag.Message = "Report Receivers.";
            ViewBag.Model = this.LoadReceivers(); ;
            return View();
        }

        [HttpPost]
        public ActionResult Subscribe(ReceiverViewModel model)
        {
            this.CreateSubscription(model);
            return Redirect(Request.UrlReferrer.ToString());
        }

        private const string receiversFile = @"C:\Users\rafaellandim\source\repos\SpaceAppsChallenge\SpaceAppsChallenge\bin\Debug\Data\receivers.json";
        public void CreateSubscription(ReceiverViewModel receiver)
        {
            List<ReceiverViewModel> allReceivers = this.LoadReceivers();
            allReceivers.Add(receiver);

            using (TextWriter writer = new StreamWriter(receiversFile, false))
            {
                writer.WriteLine(new JavaScriptSerializer().Serialize(allReceivers));
            }
        }

        public List<ReceiverViewModel> LoadReceivers()
        {
            using (StreamReader reader = new StreamReader(receiversFile))
            {
                string json = reader.ReadToEnd();
                ReceiverViewModel[] receivers =new JavaScriptSerializer().Deserialize<ReceiverViewModel[]>(json);
                return new List<ReceiverViewModel>(receivers);
            }
        }

    }
}