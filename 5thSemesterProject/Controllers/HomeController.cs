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
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];
                var id = Session["employeeId"];
                Employee employee = db.Employee.Find(id);
                ViewBag.firstWeek = 30;
				return View(employee);

                return View();
			}
			else { return RedirectToAction("../Login/Index"); }
        }

    }
}