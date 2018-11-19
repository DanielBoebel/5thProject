using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5thSemesterProject.Controllers
{
    public class HomeController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

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
        public ActionResult Employee()
        {
            ViewBag.Message = "Your employee page.";

            return View(db.Employee.ToList());
        }
        public ActionResult Delete()
        {
            ViewBag.Message = "Your employee page.";

            return View(db.Employee.ToList());
        }
    }
}