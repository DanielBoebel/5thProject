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
            if (Session["employeeId"] != null && Session["ADMINOBJ"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                int myID = 0;
                ViewBag.CurrentSort = sortOrder;
			    ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			    ViewBag.UsernameSortParm = sortOrder == "username" ? "username_desc" : "username";
			    ViewBag.TimestampSortParm = sortOrder == "timestamp" ? "timestamp_desc" : "timestamp";
			    ViewBag.ActionSortParm = sortOrder == "action" ? "action_desc" : "action";

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

                if (!String.IsNullOrEmpty(searchString))
                {
                    try
                    {
                        myID = Int32.Parse(searchString);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    if (myID != 0)
                    {
                        logs = logs.Where(s => s.log_id == myID);
                    }
                    else
                    {
                        logs = logs.Where(s => s.action.ToUpper().Contains(searchString.ToUpper()) || s.employee_name.ToUpper().Contains(searchString.ToUpper()) ||
                        s.timestamp.ToString().ToUpper().Contains(searchString.ToUpper()));
                    }
                }

            

			    switch (sortOrder)
			    {
				    case "Name":
					    logs = logs.OrderBy(s => s.employee_name);
					    break;
				    case "username_desc":
					    logs = logs.OrderByDescending(s => s.employee_name);
					    break;
				    case "action":
					    logs = logs.OrderBy(s => s.action);
					    break;
				    case "action_desc":
					    logs = logs.OrderByDescending(s => s.action);
					    break;
				    case "timestamp":
					    logs = logs.OrderBy(s => s.timestamp);
					    break;
				    case "timestamp_desc":
					    logs = logs.OrderByDescending(s => s.timestamp);
					    break;
				    default:
					    logs = logs.OrderByDescending(s => s.timestamp);
					    break;
			    }

			    int pageSize = 25;
			    int pageNumber = (page ?? 1);
			    return View(logs.ToPagedList(pageNumber, pageSize));
            }
			else { 
                return RedirectToAction("../Login/Index");
            }
        }
	}
}
