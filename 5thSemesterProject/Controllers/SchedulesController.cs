using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // Date used for showing the correct schedule
        private double ShowingDate = 0.0;

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

        public ActionResult CalendarMonth() {

            return View();
        }

        public ActionResult CalendarWeek()
        {

            return View();
        }

        // GET: Schedules for a day
        public ActionResult CalendarDay()
        {
            Console.WriteLine(ShowingDate);
            TempData["showingDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            var schedule = db.Schedule.Include(s => s.Employee).Include(s => s.Shift);
            return View(schedule.ToList());
        }

        [HttpPost]
        public ActionResult NextDay()
        {
            ShowingDate += 1;
            Console.WriteLine(ShowingDate);
            TempData["showingDate"] = DateTime.Now.AddDays(ShowingDate).ToString("dd/MM/yyyy");
            var schedule = db.Schedule.Include(s => s.Employee).Include(s => s.Shift);
            return View("../Schedules/CalendarDay");
        }

        [HttpPost]
        public ActionResult PrevDay()
        {
            ShowingDate -= 1;
            Console.WriteLine(ShowingDate);
            TempData["showingDate"] = DateTime.Now.AddDays(ShowingDate).ToString("dd/MM/yyyy");
            var schedule = db.Schedule.Include(s => s.Employee).Include(s => s.Shift);
            return View("../Schedules/CalendarDay");
        }

        public JsonResult ScheduleList(string date)
        {
            var result = from r in db.Schedule
                         where r.date.Equals("28-11-2018")
                         select new { r.date, r.Employee.firstname, r.Employee.lastname, r.Employee.Position.name, r.Shift.start_time, r.Shift.end_time};
            return Json(result, JsonRequestBehavior.AllowGet);
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
