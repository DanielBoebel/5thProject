using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5thSemesterProject.Controllers
{
    public class LoginController : Controller
    {

		private DB5thSemesterEntities db = new DB5thSemesterEntities();

		// GET: Login
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult Employee()
		{
			ViewBag.Message = "Your employee page.";

			return View(db.Employee.ToList());
		}
	}
}