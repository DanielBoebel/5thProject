using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
			var dbuser = db.Employee.Where(x => x.cpr == username).FirstOrDefault().ToString();
			var dbusername = db.Employee.Where(x => x.cpr == username).Select(x => x.cpr).ToList();
			var dbpassword = db.Employee.Where(x => x.cpr == username).Select(x => x.password).ToList();
			Trace.WriteLine(dbusername[0]);
			if (dbusername[0] == username)
			{
				if (dbpassword[0] == password)
				{
					Session["USEROBJ"] = dbuser;
					Session["username"] = username;
					FormsAuthentication.SetAuthCookie(username, true);
					return RedirectToAction("../Home/Index");
				}
				else { return View(); }
			} else { return View(); }
		}


	}
}