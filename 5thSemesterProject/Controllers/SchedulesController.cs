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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "schedule_id,employee_id,shift_id,date")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedule.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
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

            return View();
        }



        // GET: Schedules for week
        public ActionResult CalendarWeek()
        {
            ViewBag.weekId = 7;

            double dayOfYear = DateTime.Now.DayOfYear / 7; 
            double weekNum = Math.Ceiling(dayOfYear);

            // Current date (DateTime format)
            DateTime dt = DateTime.Now.AddDays(-5);
            // Current dayOfWeek (String format)
            string dayOfWeek = DateTime.Now.AddDays(-5).DayOfWeek.ToString();
            // Posible days
            string[] weekDays = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
            // Days of the week difference
            int daysToAdd = 7;
            // Dates of the days in the week
            string[] weekDayDates = new string[7];

            // finds the all days of the current week and populates "weekDayDates" array
            for (int i = 0; i < weekDays.Length; i++)
            {
                daysToAdd--;
                if (weekDays[i].Equals(dayOfWeek)) // if the weekDay is equal to current weekday, get starting date and last date
                {
                    DateTime startDate = dt.AddDays(-i).Date;

                    for (int j = 0; j < 7; j++)
                    {
                        weekDayDates[j] = startDate.AddDays(j).Date.ToString("dd-MM-yyyy");
                    }
                    break;
                }
            }

            TempData["showingWeek"] = "Uge " + weekNum;
            var schedule = db.Schedule.Where(x => x.date.Equals(weekDayDates[0]));// || x.date.Equals(weekDayDates[1]) || x.date.Equals(weekDayDates[2]) || x.date.Equals(weekDayDates[3]) || x.date.Equals(weekDayDates[4]) || x.date.Equals(weekDayDates[5]) || x.date.Equals(weekDayDates[6]));
            return View(schedule.ToList());
        }

        [HttpPost]
        public ActionResult CalendarWeek(int weekId)
        {

            int tempWeek = weekId;

            //if (myDateTime >= checkDate1 && myDateTime <= checkDate2)
            //{
            //    //is between the 2 dates
            //}
            //else
            //{


            //}
            string today = DateTime.Now.ToString("dd-MM-yyyy");
            TempData["showingWeek"] = today;
            var schedule = db.Schedule.Where(x => x.date.Equals(today));
            return View(schedule.ToList());
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
