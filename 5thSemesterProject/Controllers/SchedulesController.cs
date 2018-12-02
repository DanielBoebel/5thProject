using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _5thSemesterProject.Models;

namespace _5thSemesterProject.Controllers
{

    public class SchedulesController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

        

        public ActionResult Index() {

            var schedule = db.Schedule;


            return View(schedule.ToList());
        }



        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "cpr");
            ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "schedule_id,employee_id,shift_id,date")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedule.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("CalendarMonth");
            }

            ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "cpr", schedule.employee_id);
            ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name", schedule.shift_id);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "cpr", schedule.employee_id);
            ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name", schedule.shift_id);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "schedule_id,employee_id,shift_id,date")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "cpr", schedule.employee_id);
            ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name", schedule.shift_id);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedule.Find(id);
            db.Schedule.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Schedules for month
        public ActionResult CalendarMonth() {

			int daysOfMonthConver = 0;
			int monthConvert = 0;
			int dateToday = 0;
			string dateTodayString = "";
			string monthString = "";
			int day1 = 0;

			DateTime date = DateTime.Today;
			var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
			var firstDayOfMonthFormatted = new DateTime(date.Year, date.Month, 1).ToString("dd-MM-yyyy");
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).ToString("dd");

			Int32.TryParse(lastDayOfMonth, out daysOfMonthConver);
			var dayOfWeek = firstDayOfMonth.DayOfWeek;
			ViewBag.monthId = 0;
			ViewBag.numbOfDaysInMonth = daysOfMonthConver;
			dateTodayString = date.ToString("dd");

			Int32.TryParse(dateTodayString, out dateToday);
			ViewBag.currentDate = dateToday;

			day1 = (int)firstDayOfMonth.DayOfWeek;
			ViewBag.startDate = day1;

			string month = DateTime.Now.Month.ToString();
			Int32.TryParse(month, out monthConvert);
			switch (monthConvert)
			{
				case 1:
					monthString = "Januar";
					break;
				case 2:
					monthString = "Februar";
					break;
				case 3:
					monthString = "Marts";
					break;
				case 4:
					monthString = "April";
					break;
				case 5:
					monthString = "Maj";
					break;
				case 6:
					monthString = "Juni";
					break;
				case 7:
					monthString = "Juli";
					break;
				case 8:
					monthString = "August";
					break;
				case 9:
					monthString = "September";
					break;
				case 10:
					monthString = "October";
					break;
				case 11:
					monthString = "November";
					break;
				case 12:
					monthString = "December";
					break;
			}

			var schedule = db.Schedule.Where(x => x.date.Substring(3, 2) == month);

			TempData["showingMonth"] = monthString;

			return View(schedule.ToList());
        }

		[HttpPost]
		public ActionResult CalendarMonth(int monthId)
		{
			int daysOfMonthConver = 0;
			int monthConvert = 0;
			string monthString = "";
			int day1 = 0;
			int monthTemp = monthId;
			int dateToday = 0;
			string dateTodayString = "";

			//DateTime dateMonth = DateTime.Today.AddMonths(monthTemp);
			DateTime date = DateTime.Today.AddMonths(monthTemp);
			string month = DateTime.Now.AddMonths(monthTemp).ToString("MM");

			var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
			var firstDayOfMonthFormatted = new DateTime(date.Year, date.Month, 1).ToString("dd-MM-yyyy");
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).ToString("dd");

			dateTodayString = date.ToString("dd");

			Int32.TryParse(dateTodayString, out dateToday);
			ViewBag.currentDate = dateToday;


			Int32.TryParse(lastDayOfMonth, out daysOfMonthConver);
			var dayOfWeek = firstDayOfMonth.DayOfWeek;


			ViewBag.monthId = 0;
			ViewBag.numbOfDaysInMonth = daysOfMonthConver;
			ViewBag.monthId = monthTemp;

			day1 = (int)firstDayOfMonth.DayOfWeek;
			ViewBag.startDate = day1;


			Int32.TryParse(month, out monthConvert);

			switch (monthConvert)
			{
				case 1:
					monthString = "Januar";
					break;
				case 2:
					monthString = "Februar";
					break;
				case 3:
					monthString = "Marts";
					break;
				case 4:
					monthString = "April";
					break;
				case 5:
					monthString = "Maj";
					break;
				case 6:
					monthString = "Juni";
					break;
				case 7:
					monthString = "Juli";
					break;
				case 8:
					monthString = "August";
					break;
				case 9:
					monthString = "September";
					break;
				case 10:
					monthString = "October";
					break;
				case 11:
					monthString = "November";
					break;
				case 12:
					monthString = "December";
					break;
			}
			
			TempData["showingMonth"] = monthString;
			var schedule = db.Schedule.Where(x => x.date.Substring(3,2) == month);

			return View(schedule.ToList());
		}




        // GET: Schedules for week
        public ActionResult CalendarWeek()
        {
            double dayOfYear = DateTime.Now.DayOfYear / 7;
            double weekNum = Math.Ceiling(dayOfYear);
            ViewBag.weekId = weekNum;
            ViewBag.diff = 0;

            // Current dayOfWeek (String format)
            string dayOfWeek = DateTime.Now.AddDays(0).DayOfWeek.ToString();

            // Array of dates of current week - Contains the first date of the week on index 0 and last date of the week on index 6
            string[] dates = getDatesOfWeek(dayOfWeek, 0);

            // Because LINQ is not smart
            string monday = dates[0];
            string tuesday = dates[1];
            string wednesday = dates[2];
            string thursday = dates[3];
            string friday = dates[4];
            string saturday = dates[5];
            string sunday = dates[6];

            // Because spaghetti works
            ViewBag.monday = monday;
            ViewBag.tuesday = tuesday;
            ViewBag.wednesday = wednesday;
            ViewBag.thursday = thursday;
            ViewBag.friday = friday;
            ViewBag.saturday = saturday;
            ViewBag.sunday = sunday;

            TempData["showingWeek"] = "Uge " + weekNum;
            var schedule = db.Schedule.Where(x => x.date.Equals(monday) || x.date.Equals(tuesday) || x.date.Equals(wednesday) || x.date.Equals(thursday) || x.date.Equals(friday) || x.date.Equals(saturday) || x.date.Equals(sunday));
            return View(schedule.ToList());
        }

        [HttpPost]
        public ActionResult CalendarWeek(int weekId, int diff)
        {
            if (weekId > 52) weekId = 1;
            if (weekId < 1) weekId = 52;

            ViewBag.weekId = weekId;
            ViewBag.diff = diff;

            // Current dayOfWeek (String format)
            string dayOfWeek = DateTime.Now.AddDays(7 * diff).DayOfWeek.ToString();

            // Array of dates of current week - Contains the first date of the week on index 0 and last date of the week on index 6
            string[] dates = getDatesOfWeek(dayOfWeek, 7 * diff);

            //double dayOfYear = DateTime.Now.AddDays(weekId).DayOfYear / 7;
            //double weekNum = Math.Ceiling(dayOfYear);

            // Because LINQ is not smart
            string monday = dates[0];
            string tuesday = dates[1];
            string wednesday = dates[2];
            string thursday = dates[3];
            string friday = dates[4];
            string saturday = dates[5];
            string sunday = dates[6];

            // Because spaghetti works
            ViewBag.monday = monday;
            ViewBag.tuesday = tuesday;
            ViewBag.wednesday = wednesday;
            ViewBag.thursday = thursday;
            ViewBag.friday = friday;
            ViewBag.saturday = saturday;
            ViewBag.sunday = sunday;

            TempData["showingWeek"] = "Uge " + weekId;
            var schedule = db.Schedule.Where(x => x.date.Equals(monday) || x.date.Equals(tuesday) || x.date.Equals(wednesday) || x.date.Equals(thursday) || x.date.Equals(friday) || x.date.Equals(saturday) || x.date.Equals(sunday));
            return View(schedule.ToList());
        }

        // Returns array with dates of desired week. dayOfWeek and addDay has to have the same number added 
        public string[] getDatesOfWeek(string dayOfWeek, int addDay)
        {
            // Current date (DateTime format)
            DateTime dt = DateTime.Now.AddDays(addDay);
            // Posible days
            string[] weekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            // Days of the week difference
            int daysToAdd = 7;
            // Dates of the days in the week
            string[] weekDayDates = new string[7];

            // finds the all days of the current week and populates "weekDayDates" array
            for (int i = 0; i < weekDays.Length; i++)
            {
                daysToAdd--;
                if (weekDays[i].Equals(dayOfWeek)) // if the weekDay is equal to current weekday, get start date -> last date
                {
                    DateTime startDate = dt.AddDays(-i).Date;

                    for (int j = 0; j < 7; j++)
                    {
                        weekDayDates[j] = startDate.AddDays(j).Date.ToString("dd-MM-yyyy");
                    }
                    break;
                }
            }
            return weekDayDates;
        }

        // GET: Schedules for today
        public ActionResult CalendarDay()
        {
			ViewBag.dayId = 0;
            string today = DateTime.Now.ToString("dd-MM-yyyy");
            TempData["showingDate"] = today;
            var schedule = db.Schedule.Where(x => x.date.Equals(today));
            return View(schedule.ToList());
        }

		[HttpPost]
		public ActionResult CalendarDay(int dayId)
		{
			int dayTemp = dayId;

			ViewBag.dayId = dayTemp;
			string day = DateTime.Now.AddDays(dayTemp).ToString("dd-MM-yyyy");
			TempData["showingDate"] = day;
			var schedule = db.Schedule.Where(x => x.date.Equals(day));
			

			return View(schedule.ToList());
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
