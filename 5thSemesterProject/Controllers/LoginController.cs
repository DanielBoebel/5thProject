using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5thSemesterProject.Controllers
{
	public class LoginController : Controller
	{

		private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

		// GET: Login
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(string username, string password)
		{
			var testname = db.Employee.Where(x => x.cpr == username).Select(x => x.cpr).ToList();
			var dbpassword = db.Employee.Where(x => x.cpr == username).Select(x => x.password).ToList();
			Trace.WriteLine(testname[0]);
			if (testname[0] == username)
			{
				if (dbpassword[0] == password)
				{
					return RedirectToAction("../Home/Index");
				}
				else { return View(); }
			} else { return View(); }
		}


	}
}