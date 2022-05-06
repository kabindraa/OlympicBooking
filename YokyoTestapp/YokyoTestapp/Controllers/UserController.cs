using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YokyoTestapp.Models;
using System.Data.SqlClient;
namespace YokyoTestapp.Controllers
{
    public class UserController : Controller
    {
        SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=YokyoDatabase;Integrated Security=True");
        private YokyoDatabaseEntities db = new YokyoDatabaseEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.UserTable.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTable.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Password")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.UserTable.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userTable);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTable.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Password")] UserTable userTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userTable);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTable.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTable userTable = db.UserTable.Find(id);
            db.UserTable.Remove(userTable);
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
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(String UserName, String Password)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(@"Select * from UserTable where username=@UserName and Password=@Password", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("UserName", UserName);
            cmd.Parameters.AddWithValue("Password", Password);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            if (dt.Rows.Count > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public ActionResult administrator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult administrator(String UserName, String Password)
        {
            return RedirectToAction("index");//Open admin dashboard on successful login
        }
        public ActionResult audience()
        {
            return View();
        }
        public ActionResult audienceSignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult audienceSignUp(String UserName,String Password,String Contact,String Address)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"insert into audienceTable(UserName,Password,Contact,Address) values(@UserName,@Password,@Contact,@Address)",conn);
                cmd.Parameters.AddWithValue("@UserName",UserName);
                cmd.Parameters.AddWithValue("@Password",Password);
                cmd.Parameters.AddWithValue("@Contact",Contact);
                cmd.Parameters.AddWithValue("@Address",Address);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if(result>0)
                {
                    return RedirectToAction("audience");
                }
                return View();

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult audience(FormCollection frm)
        {
            String a="";
            if (frm["login"] == "Sign In")
            {
                String UserName = frm["UserName"];
                String Password = frm["Password"];
                DataTable dt=new DataTable();
                SqlCommand cmd = new SqlCommand("Select * from audienceTable where UserName=@UserName and Password=@Password",conn);
           
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);
                conn.Open();
                SqlDataReader dr=cmd.ExecuteReader();
                dt.Load(dr);
                conn.Close();
                Session["AudienceId"] = dt.Rows[0]["AudienceId"].ToString();
                if (dt.Rows.Count > 0)
                {
                    a = "AudienceDashboard";
                }
                else { return View(); }
                
            }
                
            if (frm["login"] == "Sign Up")
                a = "audienceSignUp";
            return RedirectToAction(a);
        }
        public ActionResult AudienceDashboard()
        {
            return View();
        }
        YokyoDatabaseEntities yde=new YokyoDatabaseEntities();
     
        public ActionResult home()
        {
            return View();
        }
        public ActionResult BroadCastDetails()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Select * from BroadCastDetailTable", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            return View(dt);
        }
        public ActionResult audienceBooking()
        {
            return View();
        }
        [HttpPost]
        public ActionResult audienceBooking(int DetailId,String BookingDate,String Remarks)
        {
            int audienceId = int.Parse(Session["AudienceId"].ToString());
            SqlCommand cmd = new SqlCommand(@"insert into BookingTable(UserId,DetailId,BookingDate,Remarks) 
                                                values(@UserId,@DetailId,@BookingDate,@Remarks)", conn);
            cmd.Parameters.AddWithValue("@UserId", audienceId);
            cmd.Parameters.AddWithValue("@DetailId",DetailId);
            cmd.Parameters.AddWithValue("@BookingDate",BookingDate);
            cmd.Parameters.AddWithValue("@Remarks",Remarks);
            conn.Open();
            int result=cmd.ExecuteNonQuery();
            conn.Close();
           if (result > 0)
            {
                {
                    Response.Write("Booking Successful");
                }
               
            }
            else
            {
                Response.Write("Booking Unsuccessful");
            }
                return View();
        }
        public ActionResult bookingDetails()
        {
            int audienceId = int.Parse(Session["AudienceId"].ToString());
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Select * from BookingTable where UserId=@UserId",conn);
            cmd.Parameters.AddWithValue("@UserId", audienceId);
            conn.Open();
            SqlDataReader dr=cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            return View(dt);
        }
   
    }
   
}
