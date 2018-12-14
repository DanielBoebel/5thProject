using _5thSemesterProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5thSemesterProject.Controllers
{
    public class HomeController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

        public ActionResult Index()
        {
			if (Session["employeeId"] != null)
			{
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];
                Employee employee = db.Employee.Find(id);
                var hoursOfMonth = GetHoursPerWeek(id);
                var admin_id = db.Employee.Where(x => x.Position.name == "Administrator").Select(x => x.employee_id).ToList();
                List<Message> adminMessages = new List<Message>();
                var msgList = new List<Message>();

                if (Session["ADMINOBJ"] != null)
                {
                     msgList = db.Message.Where(i => i.message_id % 2 != 0).ToList();
                } else
                {
                    msgList = db.Message.ToList();
                }

                foreach (var admin in admin_id)
                {
                    foreach (var msg in msgList)
                    {
                        if (msg.sender_id == admin && msg.reciever_id == id)
                        {
                            adminMessages.Add(msg);
                        }
                    }
                }

                HomepageViewModel homepageViewModel = new HomepageViewModel();
                homepageViewModel.employee = employee;
                homepageViewModel.adminmessageList = adminMessages.OrderByDescending(o => o.date).ToList();

				return View(homepageViewModel);
			}
			else { return RedirectToAction("../Login/Index"); }
        }

        public List<List<Schedule>> GetHoursPerWeek(int id) {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            Algorithm algorithm = new Algorithm();
            List<DateTime> datesInMonth = algorithm.GetDatesOfSchedule(firstDayOfMonth);
            var shiftInMonth = db.Schedule.Where(x => x.employee_id == id).ToList();
            List<double> hours = new List<double>();
            List<Schedule> week = new List<Schedule>();
            List<List<Schedule>> month = new List<List<Schedule>>();
            double countNight = 0.0;
            double countDay = 0.0;

            foreach (var day in datesInMonth)
            {
                foreach (var shift in shiftInMonth)
                {
                    if (shift.date.Equals(day.ToString("dd-MM-yyyy")))
                    {
                        var shiftStartTime = shift.Shift.start_time;
                        var shiftEndTime = shift.Shift.end_time;

                        if (shift.Shift.name.Contains("Aften"))
                        {
                            countNight += algorithm.GetHoursOfShift(shiftStartTime, shiftEndTime);
                        }
                        else {
                            countDay += algorithm.GetHoursOfShift(shiftStartTime, shiftEndTime);
                        }
                        ViewBag.countNight = countNight;
                        ViewBag.countDay = countDay;
                        week.Add(shift);
                    }
                }
            }
            return month;
        }
    }
}