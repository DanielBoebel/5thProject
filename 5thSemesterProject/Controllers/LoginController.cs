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
			var dbuser = "";
			List<string> dbusername = null;
			List<string> dbpassword = null;
			Boolean nonHashed = false;
			try
			{
				dbuser = db.Employee.Where(x => x.cpr == username).FirstOrDefault().ToString();
				dbusername = db.Employee.Where(x => x.cpr == username).Select(x => x.cpr).ToList();
				dbpassword = db.Employee.Where(x => x.cpr == username).Select(x => x.password).ToList();
				nonHashed = BCrypt.Net.BCrypt.Verify(password, dbpassword[0]);

			}
			catch (Exception e)
			{

			}


			try
			{
				if (dbusername[0] == username)
				{
					if (nonHashed != false)
					{
						Session["USEROBJ"] = dbuser;
						Session["username"] = username;
						FormsAuthentication.SetAuthCookie(username, true);
						string action = "Logged in";
						var timestamp = DateTime.Now;
						Log logUser = new Log(username, action, timestamp);
						db.Log.Add(logUser);
						db.SaveChanges();
						return RedirectToAction("../Home/Index");
					}
					else
					{
						TempData["msg"] = "Wrong e-mail or password";
						return View();
					}
				}
				else
				{
					TempData["msg"] = "Wrong e-mail or password";
					return View();
				}
			}
			catch (Exception e)
			{

			}
			TempData["msg"] = "Wrong e-mail or password";
			return View();
		}

		public ActionResult Logout()
		{
			string username = Session["username"].ToString();
			string action = "Logged Out";
			var timestamp = DateTime.Now;
			Log logUser = new Log(username, action, timestamp);
			db.Log.Add(logUser);
			db.SaveChanges();
			Session["USEROBJ"] = null;
			return RedirectToAction("../Login/Index");
		}


	}
}