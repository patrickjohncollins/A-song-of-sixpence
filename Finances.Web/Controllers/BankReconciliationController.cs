using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finances.Data.Banking;
using Finances.Data;
using Finances.Web.Models;

namespace Finances.Web.Controllers
{
    public class BankReconciliationController : Controller
    {
        private Entities db = Entities.GetContext();

        //
        // GET: /BankReconciliation/

        public ActionResult Index()
        {
            var bankreconciliation = from br in db.BankReconciliation.Include(b => b.AccountTransaction).Include(b => b.StatementLine).ToList()
                                     select new BankReconciliationListModel
                                     {
                                         ID = br.ID,
                                         TransactionDescription = GetTransactionDescription(br.AccountTransaction),
                                         BankStatementLineDescription = GetBankStatementLineDescription(br.StatementLine)
                                     };
            return View(bankreconciliation.ToList());
        }

        //
        // GET: /BankReconciliation/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    BankReconciliation bankreconciliation = db.BankReconciliation.Find(id);
        //    if (bankreconciliation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bankreconciliation);
        //}

        ////
        //// GET: /BankReconciliation/Create

        //public ActionResult Create()
        //{
        //    ViewBag.TransactionID = GetTransactions(0);  // TODO : This is broken
        //    ViewBag.BankStatementLineID = GetBankStatementLines(0); // TODO : This is broken
        //    return View();
        //}

        
        ////
        //// POST: /BankReconciliation/Create

        //[HttpPost]
        //public ActionResult Create(BankReconciliation bankreconciliation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.BankReconciliation.Add(bankreconciliation);
        //        UpdateTransactionDateBanked(bankreconciliation);
        //        db.SaveChanges();
        //        return RedirectToAction("Create");
        //    }

        //    ViewBag.TransactionID = GetTransactions(bankreconciliation.TransactionID);
        //    ViewBag.BankStatementLineID = GetBankStatementLines(bankreconciliation.BankStatementLineID);
        //    return View(bankreconciliation);
        //}


        //// Given a bank statement ID
        //// Find the associated bank account, and then the associated account.
        //// Only then get the list of transactions and statement lines.


        ////
        //// GET: /BankReconciliation/Create

        //public ActionResult CreateForStatement(int id)
        //{
        //    var bankAccountID = db.BankStatement.Single(bs => bs.ID == id).Account.AccountID; // Get the bank account associated to the statement.
        //    var accountID = db.Account.Single(a => a.BankAccountID == bankAccountID).ID; // Get the associated account.
        //    ViewBag.TransactionID = GetTransactions(accountID);
        //    ViewBag.BankStatementLineID = GetBankStatementLines(id);
        //    return View("Create");
        //}

        ////
        //// GET: /BankReconciliation/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    BankReconciliation bankreconciliation = db.BankReconciliation.Find(id);
        //    if (bankreconciliation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TransactionID = new SelectList(db.Transaction, "ID", "Method", bankreconciliation.TransactionID);
        //    ViewBag.BankStatementLineID = new SelectList(db.BankStatementLine, "ID", "TransType", bankreconciliation.BankStatementLineID);
        //    return View(bankreconciliation);
        //}

        ////
        //// POST: /BankReconciliation/Edit/5

        //[HttpPost]
        //public ActionResult Edit(BankReconciliation bankreconciliation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(bankreconciliation).State = EntityState.Modified;
        //        UpdateTransactionDateBanked(bankreconciliation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TransactionID = new SelectList(db.Transaction, "ID", "Method", bankreconciliation.TransactionID);
        //    ViewBag.BankStatementLineID = new SelectList(db.BankStatementLine, "ID", "TransType", bankreconciliation.BankStatementLineID);
        //    return View(bankreconciliation);
        //}

        ////
        //// GET: /BankReconciliation/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    BankReconciliation bankreconciliation = db.BankReconciliation.Find(id);
        //    if (bankreconciliation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bankreconciliation);
        //}

        ////
        //// POST: /BankReconciliation/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BankReconciliation bankreconciliation = db.BankReconciliation.Find(id);
        //    db.BankReconciliation.Remove(bankreconciliation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //SelectList GetBankStatementLines(int statementID, int? selectedValue = null)
        //{
        //    var items = from bsl in db.BankStatementLine.ToList()
        //                where !bsl.Preexisting
        //                && bsl.BankStatementID == statementID
        //                && bsl.BankReconciliations.Count == 0 // TODO : This only valid for creation, when modifying we want all.  Actually, modification doesn't make sense.
        //                //&& bsl.TransType == "DEBIT" // TODO : Filter needed
        //                orderby bsl.Date
        //                select new
        //                {
        //                    ID = bsl.ID,
        //                    Description = GetBankStatementLineDescription(bsl)
        //                };

        //    if (selectedValue.HasValue)
        //        return new SelectList(items, "ID", "Description", selectedValue.Value);
        //    else
        //        return new SelectList(items, "ID", "Description");
        //}

        string GetBankStatementLineDescription(BankStatementLine bsl)
        {
            return bsl.Date.ToShortDateString() + " " + bsl.Amount.ToString("C").PadLeft(10, ' ') + " " + bsl.Name + " " + bsl.Memo;
        }

        //SelectList GetTransactions(int accountID, int? selectedValue = null)
        //{
        //    var items = from t in db.Transaction.ToList()
        //                orderby t.Date
        //                where !t.Banked.HasValue
        //                && t.AccountID == accountID
        //                //&& t.Method == "Prelevement" // TODO : Filter
        //                select new
        //                {
        //                    ID = t.ID,
        //                    Description = GetTransactionDescription(t)
        //                };

        //    if (selectedValue.HasValue)
        //        return new SelectList(items, "ID", "Description", selectedValue.Value);
        //    else
        //        return new SelectList(items, "ID", "Description");
        //}

        string GetTransactionDescription(AccountTransaction t)
        {
            return t.Transaction.Date.ToShortDateString() + " " + t.Total.ToString("C") + " " + t.Transaction.Method + " " + (t.Transaction.Method == "Chèque" ? t.Transaction.ChequeNumber + " " : "") + t.Transaction.Payee + " " + t.Transaction.Nature;
        }

        //void UpdateTransactionDateBanked(BankReconciliation bankreconciliation)
        //{
        //    var transaction = db.Transaction.Single(t => t.ID == bankreconciliation.TransactionID);
        //    var bankStatementLine = db.BankStatementLine.Single(bsl => bsl.ID == bankreconciliation.BankStatementLineID);
        //    transaction.Banked = bankStatementLine.Date;
        //    transaction.Budget = false; // Transaction may no longer be a budget once reconciled.
        //}

        //public ActionResult ForStatement(int id)
        //{

        //    var list = db.BankReconciliation.Where(br => br.StatementLine.BankStatementID == id).Include("Transaction").Include("StatementLine");



        //    //var list = new List<BankReconciliation>();
        //    //list.AddRange(db.BankReconciliation.Take(20));
        //    return View(list);
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}