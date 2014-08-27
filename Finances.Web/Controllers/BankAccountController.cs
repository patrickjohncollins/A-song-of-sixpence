using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finances.Data.Banking;
using Finances.Data;

namespace Finances.Web.Controllers
{
    public class BankAccountController : Controller
    {
        private Entities db = Entities.GetContext();

        //
        // GET: /BankAccount/

        public ActionResult Index()
        {
            return View(db.BankAccount.ToList());
        }

        //
        // GET: /BankAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            BankAccount bankaccount = db.BankAccount.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        //
        // GET: /BankAccount/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BankAccount/Create

        [HttpPost]
        public ActionResult Create(BankAccount bankaccount)
        {
            if (ModelState.IsValid)
            {
                db.BankAccount.Add(bankaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bankaccount);
        }

        //
        // GET: /BankAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BankAccount bankaccount = db.BankAccount.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        //
        // POST: /BankAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(BankAccount bankaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankaccount);
        }

        //
        // GET: /BankAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BankAccount bankaccount = db.BankAccount.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        //
        // POST: /BankAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankaccount = db.BankAccount.Find(id);
            db.BankAccount.Remove(bankaccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}