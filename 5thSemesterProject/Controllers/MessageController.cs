﻿using _5thSemesterProject.Models;
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
			model.employeeList = employeeList;

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
			Int32.TryParse(Session["employeeId"].ToString(), out sender_id);
			ViewBag.recieverId = reciever_id;
			MessageViewModel model = new MessageViewModel();
			model.employeeList = new List<Employee>();
			model.messageSenderList = new List<Message>();
			model.messageRecieverList = new List<Message>();


			List<Message> listSender = new List<Message>();
			List<Message> listReciever = new List<Message>();
			List<CorrectOrder> listCorrectOrder = new List<CorrectOrder>();
			listSender = db.Message.Where(x => x.sender_id == sender_id ).ToList();
			listReciever = db.Message.Where(x => x.reciever_id == sender_id).ToList();
			List<Employee> employeeList = db.Employee.ToList();
			model.employeeList = employeeList;
			model.messageSenderList = listSender;
			model.messageRecieverList = listReciever;

			
			return View(model);
		}


		//Sending a message
		[HttpPost]
		public ActionResult _MessageContent(int reciever_id, string content)
		{


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
			db.Message.Add(message);
			db.SaveChanges();
			return View(model);
		}

    }
}