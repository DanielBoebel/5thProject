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
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                MessageViewModel model = new MessageViewModel();
			    model.employeeList = new List<Employee>();
			    model.messageSenderList = new List<Message>();
			    model.messageRecieverList = new List<Message>();


			    List<Employee> employeeList = db.Employee.ToList();
			    model.employeeList = employeeList;
                


                List<Message> messageSenderList = db.Message.ToList();
			    List<Message> messageReciverList = db.Message.ToList();

                employeeList.OrderByDescending(x => x.employee_id);

                List<Employee> tempEmployeeList = db.Employee.ToList();
                foreach (var item in tempEmployeeList)
                {
                    if (item.Position.name.Equals("Administrator"))
                    {
                        employeeList.Insert(0, item);
                    }
                    if (item.employee_id.Equals(employeeid)) {
                        employeeList.Remove(item);
                    }
                }


                //employeeList.Insert(0, employeeList[employeeList.Count -1]);

			    model.employeeList = employeeList;
			    model.messageSenderList = messageSenderList;
			    model.messageSenderList = messageReciverList;

                return View(model);
            }
            else
            {
                return RedirectToAction("../Login/Index");
            }
        }

		//Get Message content for specific person
		[HttpGet]
		public ActionResult _MessageContent(int reciever_id)
		{
            if (Session["employeeId"] != null )
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                int sender_id = 0;
			    Int32.TryParse(Session["employeeId"].ToString(), out sender_id);
			    ViewBag.recieverId = reciever_id;
			    MessageViewModel model = new MessageViewModel();
			    model.employeeList = new List<Employee>();
			    model.messageSenderList = new List<Message>();
			    model.messageRecieverList = new List<Message>();

			    List<Message> listSender = new List<Message>();
			    List<Message> listReciever = new List<Message>();
                List<Message> allMessages = new List<Message>();
                List<CorrectOrder> listCorrectOrder = new List<CorrectOrder>();
			    listSender = db.Message.Where(x => x.sender_id == sender_id ).ToList();
                foreach (var hest in listSender)
                {
                    Message messageSend = new Message(hest.message_id, hest.reciever_id, hest.date, hest.content, hest.sender_id, true);
                    allMessages.Add(messageSend);
                }

                listReciever = db.Message.Where(x => x.reciever_id == sender_id).ToList();
                foreach (var hest in listReciever)
                {
                    Message messageReceive = new Message(hest.message_id, hest.reciever_id, hest.date, hest.content, hest.sender_id, false);
                    allMessages.Add(messageReceive);
                }
                List<Message> sortedMessages = new List<Message>(allMessages.OrderBy(x => x.message_id));
                //allMessages.OrderBy(x => x.message_id);
                //allMessages.OrderBy(x => x.message_id);

                List<Employee> employeeList = db.Employee.ToList();

                List<Employee> tempEmployeeList = db.Employee.ToList();
                foreach (var item in tempEmployeeList)
                {
                    if (item.Position.name.Equals("Administrator"))
                    {
                        employeeList.Insert(0, item);
                    }
                }

                model.employeeList = employeeList;
			    model.messageSenderList = sortedMessages;
			    model.messageRecieverList = listReciever;

                return View(model);
            }
			else { 
                return RedirectToAction("../Login/Index");
            }
        }


		//Sending a message
		[HttpPost]
		public ActionResult _MessageContent(int reciever_id, string content)
		{
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                int sender_id = 0;
			    Int32.TryParse(Session["employeeId"].ToString(), out sender_id);
			    ViewBag.recieverId = reciever_id;


			    MessageViewModel model = new MessageViewModel();
			    model.employeeList = new List<Employee>();
			    model.messageSenderList = new List<Message>();
			    model.messageRecieverList = new List<Message>();


			    List<Employee> employeeList = db.Employee.ToList();
			    List<Message> listSender = new List<Message>();
			    List<Message> listReciever = new List<Message>();

                listSender = db.Message.Where(x => x.sender_id == sender_id).ToList();
            
                listReciever = db.Message.Where(x => x.reciever_id == sender_id).ToList();
            

                model.messageSenderList = listSender;
			    model.messageRecieverList = listReciever;
			    model.employeeList = employeeList;


			    DateTime timestamp = DateTime.Now;
			    Message message = new Message(sender_id, reciever_id, timestamp.ToString("dd/MM/yyyy HH:mm"), content);

                var admin_id = db.Employee.Where(x => x.Position.name == "Administrator").Select(o => o.employee_id).ToList();

                if (sender_id.Equals(admin_id[0])) {
                    foreach (var item in employeeList)
                    {
                        Message adminMessage = new Message(sender_id, item.employee_id, timestamp.ToString("dd/MM/yyyy HH:mm"), content);
                        db.Message.Add(adminMessage);
                    }
                }
			    db.Message.Add(message);
			    db.SaveChanges();
			    return View(model);
            }
			else { 
                return RedirectToAction("../Login/Index");
            }
        }

    }
}