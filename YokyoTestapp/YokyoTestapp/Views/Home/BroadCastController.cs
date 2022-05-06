using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YokyoTestapp.Models;

namespace YokyoTestapp.Views.Home
{
    public class BroadCastController : Controller
    {
        private YokyoDatabaseEntities db = new YokyoDatabaseEntities();

        // GET: BroadCast
        public ActionResult Index()
        {
            return View(db.BroadCastDetailTable.ToList());
        }

        // GET: BroadCast/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BroadCastDetailTable broadCastDetailTable = db.BroadCastDetailTable.Find(id);
            if (broadCastDetailTable == null)
            {
                return HttpNotFound();
            }
            return View(broadCastDetailTable);
        }

        // GET: BroadCast/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BroadCast/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetailId,GameType,GameName,BroadCastDate,Remarks")] BroadCastDetailTable broadCastDetailTable)
        {
            if (ModelState.IsValid)
            {
                db.BroadCastDetailTable.Add(broadCastDetailTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(broadCastDetailTable);
        }

        // GET: BroadCast/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BroadCastDetailTable broadCastDetailTable = db.BroadCastDetailTable.Find(id);
            if (broadCastDetailTable == null)
            {
                return HttpNotFound();
            }
            return View(broadCastDetailTable);
        }

        // POST: BroadCast/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DetailId,GameType,GameName,BroadCastDate,Remarks")] BroadCastDetailTable broadCastDetailTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(broadCastDetailTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(broadCastDetailTable);
        }

        // GET: BroadCast/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BroadCastDetailTable broadCastDetailTable = db.BroadCastDetailTable.Find(id);
            if (broadCastDetailTable == null)
            {
                return HttpNotFound();
            }
            return View(broadCastDetailTable);
        }

        // POST: BroadCast/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BroadCastDetailTable broadCastDetailTable = db.BroadCastDetailTable.Find(id);
            db.BroadCastDetailTable.Remove(broadCastDetailTable);
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
