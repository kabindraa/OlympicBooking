using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YokyoTestapp.Models;

namespace YokyoTestapp.Controllers
{
    public class BookingController : Controller
    {
        private YokyoDatabaseEntities db = new YokyoDatabaseEntities();

        // GET: Booking
        public ActionResult Index()
        {
            var bookingTable = db.BookingTable.Include(b => b.BroadCastDetailTable).Include(b => b.UserTable);
            return View(bookingTable.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTable bookingTable = db.BookingTable.Find(id);
            if (bookingTable == null)
            {
                return HttpNotFound();
            }
            return View(bookingTable);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            ViewBag.DetailId = new SelectList(db.BroadCastDetailTable, "DetailId", "GameType");
            ViewBag.UserId = new SelectList(db.UserTable, "UserId", "UserName");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,UserId,DetailId,BookingDate,Remarks")] BookingTable bookingTable)
        {
            if (ModelState.IsValid)
            {
                db.BookingTable.Add(bookingTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DetailId = new SelectList(db.BroadCastDetailTable, "DetailId", "GameType", bookingTable.DetailId);
            ViewBag.UserId = new SelectList(db.UserTable, "UserId", "UserName", bookingTable.UserId);
            return View(bookingTable);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTable bookingTable = db.BookingTable.Find(id);
            if (bookingTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.DetailId = new SelectList(db.BroadCastDetailTable, "DetailId", "GameType", bookingTable.DetailId);
            ViewBag.UserId = new SelectList(db.UserTable, "UserId", "UserName", bookingTable.UserId);
            return View(bookingTable);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,UserId,DetailId,BookingDate,Remarks")] BookingTable bookingTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DetailId = new SelectList(db.BroadCastDetailTable, "DetailId", "GameType", bookingTable.DetailId);
            ViewBag.UserId = new SelectList(db.UserTable, "UserId", "UserName", bookingTable.UserId);
            return View(bookingTable);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTable bookingTable = db.BookingTable.Find(id);
            if (bookingTable == null)
            {
                return HttpNotFound();
            }
            return View(bookingTable);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingTable bookingTable = db.BookingTable.Find(id);
            db.BookingTable.Remove(bookingTable);
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
