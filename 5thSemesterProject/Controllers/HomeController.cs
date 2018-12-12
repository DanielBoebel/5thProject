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
			if (Session["employeeId"] != null || Session["ADMINOBJ"] != null)
			{
                var id = Session["employeeId"];
                Employee employee = db.Employee.Find(id);
                ViewBag.firstWeek = 30;
				return View(employee);
			}
			else { return RedirectToAction("../Login/Index"); }
        }

        public ActionResult About()
        {
			if (Session["employeeId"] != null || Session["ADMINOBJ"] != null)
			{
				ViewBag.Message = "Your application description page.";

				return View();
			}
			else { return RedirectToAction("../Login/Index"); }
			
        }

        public ActionResult Contact()
        {
			if (Session["employeeId"] != null || Session["ADMINOBJ"] != null)
			{
				ViewBag.Message = "Your contact page.";

				return View();
			}
			else { return RedirectToAction("../Login/Index"); }

        }
        public ActionResult Employee()
        {
			if (Session["employeeId"] != null || Session["ADMINOBJ"] != null)
			{
				ViewBag.Message = "Your employee page.";

				return View(db.Employee.ToList());
			}
			else { return RedirectToAction("../Login/Index"); }

        }
        public ActionResult Delete()
        {
			if (Session["employeeId"] != null || Session["ADMINOBJ"] != null)
			{
				ViewBag.Message = "Your employee page.";

				return View(db.Employee.ToList());
			}
			else { return RedirectToAction("../Login/Index"); }

        }
    }
}