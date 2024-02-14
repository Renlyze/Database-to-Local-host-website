using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Table table)
        {
            if (ModelState.IsValid)
            {
                DateTime parsedDateOfBirth;

                // Check if the date is already in the correct format (MM/dd/yyyy)
                if (!DateTime.TryParseExact(table.DateOfBirth.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDateOfBirth))
                {
                    // If not, try to parse with additional formats or handle the error appropriately
                    if (!DateTime.TryParse(table.DateOfBirth.ToString(), out parsedDateOfBirth))
                    {
                        // Handle invalid date format
                        ModelState.AddModelError("DateOfBirth", "Invalid date format");
                        return View(table);
                    }
                }

                table.DateOfBirth = parsedDateOfBirth;

                // Calculate quote based on provided guidelines
                table.Quote = 50; // Base amount

                // Age calculation
                int age = DateTime.Now.Year - table.DateOfBirth.Year;
                if (age <= 18)
                    table.Quote += 100;
                else if (age >= 19 && age <= 25)
                    table.Quote += 50;
                else
                    table.Quote += 25;

                // Car year calculation
                if (table.CarYear < 2000)
                    table.Quote += 25;
                else if (table.CarYear > 2015)
                    table.Quote += 25;

                // Car make calculation
                if (table.CarMake.ToLower() == "porsche")
                {
                    table.Quote += 25;

                    // Car model calculation
                    if (table.CarModel.ToLower() == "911 carrera")
                        table.Quote += 25;
                }

                // Speeding tickets calculation
                table.Quote += 10 * table.SpeedingTickets;

                // DUI calculation
                if (table.DUI)
                    table.Quote += table.Quote + (table.Quote * 0.25m);

                // Full coverage calculation
                if (table.CoverageType)
                    table.Quote += table.Quote + (table.Quote * 0.5m);

                // Continue with the rest of your code
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Table table)
        {
            if (ModelState.IsValid)
            {
                // Ensure the DateOfBirth is in the correct format
                DateTime parsedDateOfBirth;
                if (!DateTime.TryParse(table.DateOfBirth.ToString(), out parsedDateOfBirth))
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date format");
                    return View(table);
                }

                table.DateOfBirth = parsedDateOfBirth;

                // Calculate quote based on provided guidelines
                table.Quote = 50; // Base amount

                // Age calculation
                int age = DateTime.Now.Year - table.DateOfBirth.Year;
                if (age <= 18)
                    table.Quote += 100;
                else if (age >= 19 && age <= 25)
                    table.Quote += 50;
                else
                    table.Quote += 25;

                // Car year calculation
                if (table.CarYear < 2000)
                    table.Quote += 25;
                else if (table.CarYear > 2015)
                    table.Quote += 25;

                // Car make calculation
                if (table.CarMake.ToLower() == "porsche")
                {
                    table.Quote += 25;

                    // Car model calculation
                    if (table.CarModel.ToLower() == "911 carrera")
                        table.Quote += 25;
                }

                // Speeding tickets calculation
                table.Quote += 10 * table.SpeedingTickets;

                // DUI calculation
                if (table.DUI)
                    table.Quote += table.Quote + (table.Quote * 0.25m);

                // Full coverage calculation
                if (table.CoverageType)
                    table.Quote += table.Quote + (table.Quote * 0.5m);

                // Continue with the rest of your code
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            db.Tables.Remove(table);
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
