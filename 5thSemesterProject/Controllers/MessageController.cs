using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			MessageViewModel model = new MessageViewModel();
			model.employeeList = new List<Employee>();
			model.messageSenderList = new List<Message>();
			model.messageRecieverList = new List<Message>();


			List<Employee> employeeList = db.Employee.ToList();
			List<Message> messageSenderList = db.Message.ToList();
			List<Message> messageReciverList = db.Message.ToList();

			model.employeeList = employeeList;
			model.messageSenderList = messageSenderList;
			model.messageSenderList = messageReciverList;


			return View(model);
        }

		//Get Message content for specific person
		[HttpGet]
		public ActionResult _MessageContent(int reciever_id)
		{
			int sender_id = 0;
			//Int32.TryParse(Session["employeeId"].ToString(), out sender_id);
			MessageViewModel model = new MessageViewModel();
			model.employeeList = new List<Employee>();
			model.messageSenderList = new List<Message>();
			model.messageRecieverList = new List<Message>();


			List<Message> listSender = new List<Message>();
			List<Message> listReciever = new List<Message>();
			listSender = db.Message.Where(x => x.sender_id == reciever_id).ToList();
			listReciever = db.Message.Where(x => x.reciever_id == reciever_id).ToList();
			List<Employee> employeeList = db.Employee.ToList();
			model.employeeList = employeeList;
			model.messageSenderList = listSender;
			model.messageRecieverList = listReciever;

			return View(model);
		}
    }
}