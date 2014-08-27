using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finances.Data.Banking;
using Finances.Data;
using Finances.Logic;
using Finances.Web.Models;



namespace Finances.Web.Controllers
{
    public class BankStatementController : Controller
    {
        private Entities db = Entities.GetContext();

        //
        // GET: /BankStatement/

        //public ActionResult Index()
        //{
        //    var bankstatement = db.BankStatement.Include(b => b.Account);
        //    ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID");
        //    return View(bankstatement.ToList());
        //}

        public ActionResult Index()
        {
            var bankstatement = db.BankStatement.Include(b => b.Account);
            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID");
            return View(bankstatement.ToList());
        }

        public ActionResult ForAccount(int id)
        {
            var bankstatement = db.BankStatement.Include(b => b.Account).Where(bs => bs.BankAccountID == id);
            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID");
            return View("Index", bankstatement.ToList());
        }

        //
        // GET: /BankStatement/Details/5

        public ActionResult Details(int id = 0)
        {
            BankStatement bankstatement = db.BankStatement.Find(id);
            if (bankstatement == null)
            {
                return HttpNotFound();
            }
            return View(bankstatement);
        }

        //
        // GET: /BankStatement/Create

        public ActionResult Create()
        {
            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID");
            return View();
        }

        //
        // POST: /BankStatement/Create

        [HttpPost]
        public ActionResult Create(BankStatement bankstatement)
        {
            if (ModelState.IsValid)
            {
                db.BankStatement.Add(bankstatement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID", bankstatement.BankAccountID);
            return View(bankstatement);
        }

        //
        // GET: /BankStatement/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BankStatement bankstatement = db.BankStatement.Find(id);
            if (bankstatement == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID", bankstatement.BankAccountID);
            return View(bankstatement);
        }

        //
        // POST: /BankStatement/Edit/5

        [HttpPost]
        public ActionResult Edit(BankStatement bankstatement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankstatement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountID = new SelectList(db.BankAccount, "ID", "AccountID", bankstatement.BankAccountID);
            return View(bankstatement);
        }


        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var uploader = new BankStatementUploadLogic();
                var statements = uploader.UploadStatements(Request.Files[0].FileName, Request.Files[0].InputStream);
                return RedirectToAction("Reconcile", new { id = statements.FirstOrDefault().ID }); // Start reconciling immediately
            }
            return View();
        }


        //
        // GET: /BankStatement/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BankStatement bankstatement = db.BankStatement.Find(id);
            if (bankstatement == null)
            {
                return HttpNotFound();
            }
            return View(bankstatement);
        }

        //
        // POST: /BankStatement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BankStatement bankstatement = db.BankStatement.Find(id);
            db.BankStatement.Remove(bankstatement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult Reconcile(int id)
        {
            var model = new BankReconciliationCreateModel();
            GetTransactionsAndStatementLines(id, model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Reconcile(int id, BankReconciliationCreateModel model)
        {
            ValidateReconTotals(model);
            if (ModelState.IsValid)
            {
                CreateReconciliations(model);
                return RedirectToAction("Reconcile", id);
            }
            GetTransactionsAndStatementLines(id, model);
            return View(model);
        }

        void ValidateReconTotals(BankReconciliationCreateModel model)
        {
            if (!ModelState.IsValid) return; // No point testing if already invalid.
            var logic = new BankReconciliationLogic();
            if (logic.GetTotalForStatementLines(model.BankStatementLineID) !=
                logic.GetTotalForTransactions(model.AccountTransactionID))
                ModelState.AddModelError(String.Empty, "Total of selected statement lines does not equal total of selected transactions.");
        }

        void CreateReconciliations(BankReconciliationCreateModel model)
        {
            var items = MapBankReconciliationModel(model);
            foreach (var item in items)
            {
                db.BankReconciliation.Add(item);
                UpdateTransactionDateBanked(item);
            }
            db.SaveChanges();
        }

        BankReconciliation[] MapBankReconciliationModel(BankReconciliationCreateModel model)
        {
            return (
                from bsl in model.BankStatementLineID
                from t in model.AccountTransactionID
                select new BankReconciliation() {
                    BankStatementLineID = bsl,
                    AccountTransactionID = t
                }).ToArray();
        }

        void UpdateTransactionDateBanked(BankReconciliation bankreconciliation)
        {
            var at = db.AccountTransaction.Single(t => t.ID == bankreconciliation.AccountTransactionID);
            var bankStatementLine = db.BankStatementLine.Single(bsl => bsl.ID == bankreconciliation.BankStatementLineID);
            at.Banked = bankStatementLine.Date;
            at.Transaction.Budget = false; // Transaction may no longer be a budget once reconciled.
        }

        void GetTransactionsAndStatementLines(int id, BankReconciliationCreateModel model)
        {
            var bankAccountID = db.BankStatement.Single(bs => bs.ID == id).Account.AccountID; // Get the bank account associated to the statement.
            var accountID = db.Account.Single(a => a.BankAccountID == bankAccountID).ID; // Get the associated account.
            model.BankStatementLineList = GetBankStatementLines(id, model.BankStatementLineID);
            model.AccountTransactionList = GetTransactions(accountID, model.Method, model.AccountTransactionID);
            model.MethodList = GetMethods(model.Method);
        }

        IEnumerable<SelectListItem> GetBankStatementLines(int statementID, int[] bankStatementLineID)
        {
            return from bsl in db.BankStatementLine.ToList()
                    where !bsl.Preexisting
                    && bsl.BankStatementID == statementID
                    && bsl.BankReconciliations.Count == 0 // TODO : This only valid for creation, when modifying we want all.  Actually, modification doesn't make sense.
                    //&& bsl.TransType == "DEBIT" // TODO : Filter needed
                    orderby bsl.Date
                    select new SelectListItem
                    {
                        Value = bsl.ID.ToString(),
                        Text = GetBankStatementLineDescription(bsl),
                        Selected = (bankStatementLineID == null ? false : bankStatementLineID.Contains(bsl.ID))
                    };
        }

        string GetBankStatementLineDescription(BankStatementLine bsl)
        {
            return bsl.Date.ToShortDateString() + " " + bsl.Amount.ToString("C").PadLeft(10, ' ') + " " + bsl.Name + " " + bsl.Memo;
        }

        IEnumerable<SelectListItem> GetTransactions(int accountID, string method, int[] accountTransactionID)
        {
            var query = from t in db.AccountTransaction
                        orderby t.Banked.HasValue descending, t.Banked, t.Transaction.Date
                        where t.AccountID == accountID
                        && t.BankReconciliations.Count == 0
                        && (!t.Banked.HasValue)
                        select t;

            query = query.Include(at => at.Transaction);

            if (!string.IsNullOrEmpty(method))
                query = query.Where(t => t.Transaction.Method == method);

            return query.AsEnumerable().Select(t => new SelectListItem {
                Value = t.ID.ToString(),
                Text = GetTransactionDescription(t),
                Selected = (accountTransactionID == null ? false : accountTransactionID.Contains(t.ID))
            });
        }

        string GetTransactionDescription(AccountTransaction at)
        {
            return at.Banked.ToString() + " " + at.Transaction.Date.ToShortDateString() + " " + at.Total.ToString("C") + " " + at.Transaction.Method + " " + (at.Transaction.Method == "Chèque" ? at.Transaction.ChequeNumber + " " : "") + at.Transaction.Payee + " " + at.Transaction.Nature;
        }

        IEnumerable<SelectListItem> GetMethods(string method)
        {
            return db.Transaction.Select(t => t.Method).Distinct().ToList().OrderBy(m => m).Select(m => new SelectListItem() {
                Value = m,
                Text = m,
                Selected = (method == m)
            });
        }
    }
}