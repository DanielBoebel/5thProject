using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5thSemesterProject.Controllers
{
    public class MessageController : Controller
    {
		private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

		// GET: Message
		public ActionResult Index()
        {
			var employee = db.Employee;
            return View(employee.ToList());
        }

		//Get Message content for specific person
		[HttpGet]
		public ActionResult _MessageContent(string sender_id, string reciever_id)
		{
			return View();
		}
    }
}