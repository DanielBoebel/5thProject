using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Windows.Forms;
using _5thSemesterProject.Models;

namespace _5thSemesterProject.Controllers
{

    public class SchedulesController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

        public int employeeID = 0;

        public ActionResult Index() {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                var schedule = db.Schedule;
                return View(schedule.OrderBy(o => o.Employee.lastname).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }



        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

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
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "initials");
                ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name");
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
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

            ViewBag.employee_id = new SelectList(db.Employee, "employee_id", "initials", schedule.employee_id);
            ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name", schedule.shift_id);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Schedule schedule = db.Schedule.Find(id);
                if (schedule == null)
                {
                    return HttpNotFound();
                }
                ViewBag.initials = new SelectList(db.Employee, "initials", "initials", schedule.employee_id);
                ViewBag.shift_id = new SelectList(db.Shift, "shift_id", "name", schedule.shift_id);
                return View(schedule);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
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
            if (Session["employeeId"] != null)
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
            else
            {
                return RedirectToAction("../Home/Index");
            }
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
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                int daysOfMonthConver = 0;
			    int monthConvert = 0;
			    int dateToday = 0;
			    string dateTodayString = "";
			    string monthString = "";
			    int day1 = 0;
			    int employeeId = Convert.ToInt32(Session["employeeId"]);

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
			    var dates = db.Schedule.Where(x => x.employee_id == employeeId).Select(x => x.date);
			    TempData["showingMonth"] = monthString;
			    ViewBag.workingDates = dates;
			    return View(schedule.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpPost]
		public ActionResult CalendarMonth(int monthId)
		{
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                int daysOfMonthConver = 0;
			    int monthConvert = 0;
			    string monthString = "";
			    int day1 = 0;
			    int monthTemp = monthId;
			    int dateToday = 0;
			    string dateTodayString = "";
			    int employeeId = Convert.ToInt32(Session["employeeId"]);

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
					    monthString = "Oktober";
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
			    var dates = db.Schedule.Where(x => x.employee_id == employeeId).Select(x => x.date);
			    ViewBag.workingDates = dates;

			    return View(schedule.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Schedules for week
        public ActionResult CalendarWeek()
        {
            if (Session["employeeId"] != null)
            {
                employeeID = Convert.ToInt32(Session["employeeId"]);
                double dayOfYear = DateTime.Now.DayOfYear / 7.0;
                double weekNum = Math.Ceiling(dayOfYear);
                ViewBag.weekId = Convert.ToInt32(weekNum);
                ViewBag.diff = 0;

                // To showcase who is logged in
                var firstname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

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
                var schedule = db.Schedule.Where(x => x.date.Equals(monday) && x.Employee.employee_id == employeeID || x.date.Equals(tuesday) && x.Employee.employee_id == employeeID || x.date.Equals(wednesday) && x.Employee.employee_id == employeeID || x.date.Equals(thursday) && x.Employee.employee_id == employeeID || x.date.Equals(friday) && x.Employee.employee_id == employeeID || x.date.Equals(saturday) && x.Employee.employee_id == employeeID || x.date.Equals(sunday) && x.Employee.employee_id == employeeID);
                //var schedule = db.Schedule.Where(x => x.date.Equals(monday)|| x.date.Equals(tuesday) || x.date.Equals(wednesday) || x.date.Equals(thursday)  || x.date.Equals(friday)|| x.date.Equals(saturday) || x.date.Equals(sunday));
                return View(schedule.OrderBy(o => o.Employee.lastname).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult CalendarWeek(int? weekId, int? diff)
        {
            if (Session["employeeId"] != null)
            {
                employeeID = Convert.ToInt32(Session["employeeId"]);
                // To showcase who is logged in
                var firstname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                if (weekId > 52) weekId = 1;
                if (weekId < 1) weekId = 52;

                ViewBag.weekId = weekId;
                ViewBag.diff = diff;

                // Current dayOfWeek (String format)
                string dayOfWeek = DateTime.Now.AddDays(7 * Convert.ToInt32(diff)).DayOfWeek.ToString();

                // Array of dates of current week - Contains the first date of the week on index 0 and last date of the week on index 6
                string[] dates = getDatesOfWeek(dayOfWeek, 7 * Convert.ToInt32(diff));

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
                var schedule = db.Schedule.Where(x => x.date.Equals(monday) && x.Employee.employee_id == employeeID || x.date.Equals(tuesday) && x.Employee.employee_id == employeeID || x.date.Equals(wednesday) && x.Employee.employee_id == employeeID || x.date.Equals(thursday) && x.Employee.employee_id == employeeID || x.date.Equals(friday) && x.Employee.employee_id == employeeID || x.date.Equals(saturday) && x.Employee.employee_id == employeeID || x.date.Equals(sunday) && x.Employee.employee_id == employeeID);
                //var schedule = db.Schedule.Where(x => x.date.Equals(monday) || x.date.Equals(tuesday) || x.date.Equals(wednesday) || x.date.Equals(thursday) || x.date.Equals(friday) || x.date.Equals(saturday) || x.date.Equals(sunday));
                return View(schedule.OrderBy(o => o.Employee.lastname).ToList());

            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // Returns array with dates of desired week. dayOfWeek and addDay hava to have the same number added 
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
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                ViewBag.dayId = 0;
                string today = DateTime.Now.ToString("dd-MM-yyyy");
                TempData["showingDate"] = today;
                var schedule = db.Schedule.Where(x => x.date.Equals(today) && x.Employee.employee_id == id);
                return View(schedule.OrderBy(o => o.Employee.lastname).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

		[HttpPost]
		public ActionResult CalendarDay(int dayId)
		{
            if (Session["employeeId"] != null)
            {
                employeeID = Convert.ToInt32(Session["employeeId"]);
                // To showcase who is logged in
                var firstname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeID).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];
                int dayTemp = dayId;

                ViewBag.dayId = dayTemp;
                string day = DateTime.Now.AddDays(dayTemp).ToString("dd-MM-yyyy");
                TempData["showingDate"] = day;
                var schedule = db.Schedule.Where(x => x.date.Equals(day) && x.Employee.employee_id == employeeID);
                return View(schedule.OrderBy(o => o.Employee.lastname).ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        public ActionResult GenerateSchedule() {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                ViewData["Year"] = DateTime.Now.Year.ToString();
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult GenerateSchedule(string Month, int weekdayEmpDay, int weekdayEmpNight, int weekendEmpDay, int weekendEmpNight)
        {
            if (Session["employeeId"] != null)
            {
                DateTime date = Convert.ToDateTime(Month);
                DateTime firstDayOfSchedule = new DateTime(date.Year, date.Month, 1);
                DateTime lastDayOfSchedule = firstDayOfSchedule.AddMonths(1).AddDays(-1);

                DialogResult result = MessageBox.Show("Er du sikker på at du vil oprette en vagtplan fra:\n" + firstDayOfSchedule.DayOfWeek+" "+ firstDayOfSchedule.ToString("dd-MM-yyyy") +" til " + lastDayOfSchedule.DayOfWeek + " " + lastDayOfSchedule.ToString("dd-MM-yyyy"), "Bekræft", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    Algorithm x = new Algorithm();
                    x.GenerateSchedule(date, weekdayEmpDay, weekdayEmpNight, weekendEmpDay, weekendEmpNight);
                }
                if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Handling afbrudt", "Afbrudt");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
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
