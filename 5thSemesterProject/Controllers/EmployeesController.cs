using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _5thSemesterProject.Models;
using PagedList;

namespace _5thSemesterProject.Controllers
{
    public class EmployeesController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();
        
        // GET: Employees
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["employeeId"] != null)
            {
                // To showcase who is logged in
                int id = Convert.ToInt32(Session["employeeId"]);
                var firstname = db.Employee.Where(x => x.employee_id == id).Select(o => o.firstname).ToList();
                var lastname = db.Employee.Where(x => x.employee_id == id).Select(o => o.lastname).ToList();
                ViewBag.employeeLoggedIn = firstname[0] + " " + lastname[0];

                //var employee = db.Employee.Include(e => e.Position);

                int myID = 0;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewBag.FirstNameSortParm = sortOrder == "firstname" ? "firstname_desc" : "firstname";
                ViewBag.LastNameSortParm = sortOrder == "lastname" ? "lastname_desc" : "lastname";
                ViewBag.InitialsSortParm = sortOrder == "initials" ? "initials_desc" : "initials";
                ViewBag.PositionSortParm = sortOrder == "position" ? "position_desc" : "position";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var employee = from s in db.Employee.Include(e => e.Position).ToList()
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
                        employee = employee.Where(s => s.employee_id == myID);
                    }
                    else
                    {
                        employee = employee.Where(s => s.firstname.ToUpper().Contains(searchString.ToUpper()) || s.lastname.ToUpper().Contains(searchString.ToUpper()) ||
                        s.initials.ToString().ToUpper().Contains(searchString.ToUpper()) || s.Position.name.ToUpper().Contains(searchString.ToUpper()));
                    }
                }



                switch (sortOrder)
                {
                    case "firstname":
                        employee = employee.OrderBy(s => s.firstname);
                        break;
                    case "firstname_desc":
                        employee = employee.OrderByDescending(s => s.firstname);
                        break;
                    case "lastname":
                        employee = employee.OrderBy(s => s.lastname);
                        break;
                    case "lastname_desc":
                        employee = employee.OrderByDescending(s => s.lastname);
                        break;
                    case "initials":
                        employee = employee.OrderBy(s => s.initials);
                        break;
                    case "initials_desc":
                        employee = employee.OrderByDescending(s => s.initials);
                        break;
                    case "position":
                        employee = employee.OrderBy(s => s.Position.name);
                        break;
                    case "position_desc":
                        employee = employee.OrderByDescending(s => s.Position.name);
                        break;
                    default:
                        employee = employee.OrderByDescending(s => s.employee_id);
                        break;
                }

                int pageSize = 25;
                int pageNumber = (page ?? 1);
                return View(employee.ToPagedList(pageNumber, pageSize));
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
