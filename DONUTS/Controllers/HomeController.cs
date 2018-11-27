using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;
using DONUTS.Models;

namespace DONUTS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://grandcircusco.github.io/demo-apis/donuts.json");
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JObject donutJson = JObject.Parse(data);

            List<JToken> donuts = donutJson["results"].ToList();

            List<Donut> inventory = new List<Donut>();
            for(int i = 0; i<donuts.Count; i++)
            {
                Donut o = new Donut();
                o.ID = int.Parse(donuts[i]["id"].ToString());
                o.Link = donuts[i]["ref"].ToString();
                o.Name = donuts[i]["name"].ToString();

                inventory.Add(o);
            }
            rd.Close();


            return View(inventory);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Donut(int id)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://grandcircusco.github.io/demo-apis/donuts/{id}.json");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JObject donutJson = JObject.Parse(data);

            ViewBag.Name = donutJson["name"].ToString();
            ViewBag.Calories = donutJson["calories"].ToString();

            ViewBag.Extras = donutJson["extras"].ToList();

            try
            {
                ViewBag.Photo = donutJson["photo"].ToString();
            }
            catch
            {
                ViewBag.Photo = "data:image/jpeg;base64,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";
            }
            rd.Close();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}