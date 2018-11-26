using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace _5thSemesterProject.Controllers
{
    public class LogController : Controller
    {

		private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();


		// GET: Log
		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			ViewBag.UsernameSortParm = sortOrder == "username" ? "username_desc" : "username";
			ViewBag.TimestampSortParm = sortOrder == "timestamp" ? "timestamp_desc" : "timestamp";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var logs = from s in db.Log.ToList()
					   select s;

			switch (sortOrder)
			{


				case "Name":
					logs = logs.OrderBy(s => s.employee_name);
					break;
				case "username_desc":
					logs = logs.OrderByDescending(s => s.employee_name);
					break;
				case "timestamp":
					logs = logs.OrderBy(s => s.timestamp);
					break;
				case "timestamp_desc":
					logs = logs.OrderByDescending(s => s.timestamp);
					break;
				default:
					logs = logs.OrderByDescending(s => s.log_id);
					break;
			}

			int pageSize = 25;
			int pageNumber = (page ?? 1);
			return View(logs.ToPagedList(pageNumber, pageSize));
		}
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Log/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Log/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Log/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Log/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Log/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Log/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
