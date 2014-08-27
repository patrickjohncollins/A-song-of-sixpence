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
    public class BankStatementLineController : Controller
    {
        private Entities db = Entities.GetContext();

        //
        // GET: /BankStatementLine/

        public ActionResult Index()
        {
            var bankstatementline = db.BankStatementLine.Include(b => b.Statement);
            return View(bankstatementline.ToList());
        }

        public ActionResult ForStatement(int id)
        {
            var bankstatementline = db.BankStatementLine.Include(b => b.Statement).Where(bsl => bsl.BankStatementID == id);
            return View("Index", bankstatementline.ToList());
        }

        //
        // GET: /BankStatementLine/Details/5

        public ActionResult Details(int id = 0)
        {
            BankStatementLine bankstatementline = db.BankStatementLine.Find(id);
            if (bankstatementline == null)
            {
                return HttpNotFound();
            }
            return View(bankstatementline);
        }

        //
        // GET: /BankStatementLine/Create

        public ActionResult Create()
        {
            ViewBag.BankStatementID = new SelectList(db.BankStatement, "ID", "ID");
            return View();
        }

        //
        // POST: /BankStatementLine/Create

        [HttpPost]
        public ActionResult Create(BankStatementLine bankstatementline)
        {
            if (ModelState.IsValid)
            {
                db.BankStatementLine.Add(bankstatementline);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankStatementID = new SelectList(db.BankStatement, "ID", "ID", bankstatementline.BankStatementID);
            return View(bankstatementline);
        }

        //
        // GET: /BankStatementLine/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BankStatementLine bankstatementline = db.BankStatementLine.Find(id);
            if (bankstatementline == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankStatementID = new SelectList(db.BankStatement, "ID", "ID", bankstatementline.BankStatementID);
            return View(bankstatementline);
        }

        //
        // POST: /BankStatementLine/Edit/5

        [HttpPost]
        public ActionResult Edit(BankStatementLine bankstatementline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankstatementline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankStatementID = new SelectList(db.BankStatement, "ID", "ID", bankstatementline.BankStatementID);
            return View(bankstatementline);
        }

        //
        // GET: /BankStatementLine/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BankStatementLine bankstatementline = db.BankStatementLine.Find(id);
            if (bankstatementline == null)
            {
                return HttpNotFound();
            }
            return View(bankstatementline);
        }

        //
        // POST: /BankStatementLine/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BankStatementLine bankstatementline = db.BankStatementLine.Find(id);
            db.BankStatementLine.Remove(bankstatementline);
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