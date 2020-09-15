using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMODA_FRONT_HTTPS.Models;
using Newtonsoft.Json;

namespace IMODA_FRONT_HTTPS.Controllers
{
    public class CustomerController : Controller
    {
        private test1Entities db = new test1Entities();

        // GET: Customer
        public ActionResult islogin()
        {
            if (Session["member_id"] != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);  // OK = 200
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  // BadRequest = 400
            }

        }
        public JsonResult category()
        {
            var res = new JsonResult();
            //var value = "actionValue"; 
            //db.ContextOptions.ProxyCreationEnabled = false; 
            var service_category = db.service_category.Select(c => new { c.id, c.name }).ToList();
            string data = JsonConvert.SerializeObject(service_category);
            return Json(data);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult create(FormCollection post)
        {

            TempData["message"] = "success";

            return View();
        }
        
        public ActionResult get_user_request()
        {
            var id = Int32.Parse(Session["member_id"].ToString());
            var service_order = db.service_order
                .Where(b => b.user_id == id).OrderBy(e => e.id).ToList();

            //int i = 0;
            //foreach (var item in service_order)
            //{
            //    switch (item.process)
            //    {
            //        case 1:
            //            Console.WriteLine("Case 1");
            //            break;
            //        case 2:
            //            Console.WriteLine("Case 2");
            //            break;
            //        default:
            //            Console.WriteLine("Default case");
            //            break;
            //    }
            //    i++;
            //}
            string data = JsonConvert.SerializeObject(service_order);
            return Json(data);
        }
        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_category service_category = db.service_category.Find(id);
            if (service_category == null)
            {
                return HttpNotFound();
            }
            return View(service_category);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,selected,name")] service_category service_category)
        {
            if (ModelState.IsValid)
            {
                db.service_category.Add(service_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service_category);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_category service_category = db.service_category.Find(id);
            if (service_category == null)
            {
                return HttpNotFound();
            }
            return View(service_category);
        }

        // POST: Customer/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,selected,name")] service_category service_category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service_category);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_category service_category = db.service_category.Find(id);
            if (service_category == null)
            {
                return HttpNotFound();
            }
            return View(service_category);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service_category service_category = db.service_category.Find(id);
            db.service_category.Remove(service_category);
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
