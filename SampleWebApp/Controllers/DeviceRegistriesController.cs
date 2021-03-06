﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppDeviceTest.Data;
using WebAppDeviceTest.Models;

namespace WebAppDeviceTest.Controllers
{
    public class DeviceRegistriesController : Controller
    {
        // private WebAppDeviceTestContext db = new WebAppDeviceTestContext();
        AzureIoTHubContext db = new AzureIoTHubContext();

        // GET: DeviceRegistries
        public ActionResult Index()
        {
            //            return View(db.DeviceRegistries.ToList());
            return View(db.ToList());
        }

        // GET: DeviceRegistries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            DeviceRegistry deviceRegistry = db.DeviceRegistries.Find(id);
            DeviceRegistry deviceRegistry = db.Find(id.Value);
            if (deviceRegistry == null)
            {
                return HttpNotFound();
            }
            return View(deviceRegistry);
        }

        // GET: DeviceRegistries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeviceRegistries/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceId,DesiredProperties,ReportedProperties,ETags")] DeviceRegistry deviceRegistry)
        {
            if (ModelState.IsValid)
            {
                // db.DeviceRegistries.Add(deviceRegistry);
                // db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deviceRegistry);
        }

        // GET: DeviceRegistries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
//            DeviceRegistry deviceRegistry = db.DeviceRegistries.Find(id);
            DeviceRegistry deviceRegistry = db.Find(id.Value);
            if (deviceRegistry == null)
            {
                return HttpNotFound();
            }
            return View(deviceRegistry);
        }

        // POST: DeviceRegistries/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceId,DesiredProperties,ReportedProperties,ETags,MethodName")] DeviceRegistry deviceRegistry)
        {
            if (ModelState.IsValid)
            {
                //                db.Entry(deviceRegistry).State = EntityState.Modified;
                //              db.SaveChanges();
                db.Update(deviceRegistry);
                return RedirectToAction("Index");
            }
            return View(deviceRegistry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invoke([Bind(Include = "Id,DeviceId,MethodName,MethodPayload")] DeviceRegistry deviceRegistry)
        {
            var result = db.Invoke(deviceRegistry);
            DeviceRegistry listedDeviceRegistry = db.Find(deviceRegistry.Id);
            listedDeviceRegistry.MethodPayload = result;
            return RedirectToAction("Details", new { id = deviceRegistry.Id });
        }

        // POST: DeviceRegistries/Send/0
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send([Bind(Include = "Id,DeviceId,C2DMessage")] DeviceRegistry deviceRegistry)
        {
            db.Send(deviceRegistry);
            return RedirectToAction("Details", new { id = deviceRegistry.Id });
        }

        // GET: DeviceRegistries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            DeviceRegistry deviceRegistry = db.DeviceRegistries.Find(id);
            DeviceRegistry deviceRegistry = db.Find(id.Value);
            if (deviceRegistry == null)
            {
                return HttpNotFound();
            }
            return View(deviceRegistry);
        }

        // POST: DeviceRegistries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
   //         DeviceRegistry deviceRegistry = db.DeviceRegistries.Find(id);
//            db.DeviceRegistries.Remove(deviceRegistry);
  //          db.SaveChanges();
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
