using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finances.Data;

namespace Finances.Web.Controllers
{
    public class AccountController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            var accounts = GetAccounts();
            ViewBag.SumCurrentBalance = accounts.Sum(a => a.CurrentBalance);
            ViewBag.SumBankedBalance = accounts.Sum(a => a.BankedBalance);
            return View(accounts);
        }

        IEnumerable<Account> GetAccounts()
        {
            return db.Account.OrderBy(a => a.Name).AsEnumerable();
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id = 0)
        {
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Account.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        //
        // GET: /Account/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Account.Find(id);
            db.Account.Remove(account);
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