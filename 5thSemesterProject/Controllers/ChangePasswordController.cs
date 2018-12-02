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
    public class ChangePasswordController : Controller
    {
        private DB5thSemesterEntities1 db = new DB5thSemesterEntities1();

        // GET: ChangesPassword/Edit/5
        public ActionResult Edit()
        {
            if (Session["employeeId"] != null)
            {
                string idString = Session["employeeId"].ToString();
                int id = Int32.Parse(idString);
                Employee employee = db.Employee.Find(id);
                if (idString == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
                if (employee == null) return HttpNotFound();
                
				return View(employee);
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        // POST: ChangesPassword/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employee_id,cpr,firstname,lastname,position_id,special_agreement_id,initials,password")] Employee employee, string oldPassword, string newPassword, string confirmPassword)
        {
            string idString = Session["employeeId"].ToString();
            int id = Int32.Parse(idString);
            employee = db.Employee.Find(id);
            var nonHashed = BCrypt.Net.BCrypt.Verify(oldPassword, employee.password);
            if (ModelState.IsValid)
            {
                if (nonHashed == true)
                {
                    if (newPassword.Length >= 6 &&        //if length is >= 6
                        newPassword.Any(char.IsUpper) &&  //if any character is upper case
                        newPassword.Any(char.IsDigit))
                    {

                        if (newPassword == confirmPassword)
                        {
                            var newPasswordHashed = BCrypt.Net.BCrypt.HashPassword(newPassword).ToString();
                            employee.password = newPasswordHashed;
                            db.Entry(employee).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.ErrorMessage = "Du har nu skiftet kodeord";
                            //string action = "changed password";
                            //logAction(action);

                            return View(employee);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Kodeord er ikke ens";
                        }
                    }
                    else { ViewBag.ErrorMessage = "Kodeordet skal indeholde 6 tegn/Store og små bogstaver/Tal"; }
                }
                else
                {
                    ViewBag.ErrorMessage = "Indtastede kodeord er ikke det samme som det gamle";
                }

            }
            return View(employee);
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
