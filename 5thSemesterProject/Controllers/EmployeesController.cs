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
    public class EmployeesController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();
        
        // GET: Employees
        public ActionResult Index()
        {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                var employee = db.Employee.Include(e => e.Position);
                return View(employee.ToList());
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["employeeId"] != null && Session["ADMINOBJ"] != null)
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
                Employee employee = db.Employee.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            if (Session["employeeId"] != null && Session["ADMINOBJ"] != null)
            {
                // To showcase who is logged in
                int employeeid = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == employeeid).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                ViewBag.position_id = new SelectList(db.Position, "position_id", "name");

                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "employee_id,cpr,firstname,lastname,position_id,initials,password")] Employee employee, string password)
        {
			employee.password = BCrypt.Net.BCrypt.HashPassword(password).ToString();
			if (ModelState.IsValid)
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.position_id = new SelectList(db.Position, "position_id", "name", employee.position_id);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["employeeId"] != null && Session["ADMINOBJ"] != null)
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
                Employee employee = db.Employee.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                ViewBag.position_id = new SelectList(db.Position, "position_id", "name", employee.position_id);
                return View(employee);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employee_id,cpr,firstname,lastname,position_id,initials,password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.position_id = new SelectList(db.Position, "position_id", "name", employee.position_id);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["employeeId"] != null && Session["ADMINOBJ"] != null)
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
                Employee employee = db.Employee.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
