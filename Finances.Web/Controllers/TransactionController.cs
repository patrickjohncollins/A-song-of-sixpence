using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finances.Data;
using Finances.Data.Banking;
using Finances.Web.Models;
using Finances.Logic;
using AutoMapper;

namespace Finances.Web.Controllers
{
    public class TransactionController : Controller
    {
        private Entities db = Entities.GetContext();
        private TransactionLogic transLogic = new TransactionLogic();

        //
        // GET: /Transaction/

        public ActionResult Index(TransactionListModel model)
        {
            var query = db.AccountTransaction
                .OrderBy(t => t.Banked.HasValue)
                .ThenByDescending(t => t.Banked)
                .ThenByDescending(t => t.Transaction.Date)
                .ThenByDescending(t => t.ID)
                .Select(t => t);

            if (model.AccountID.HasValue)
                query = query.Where(t => t.AccountID == model.AccountID.Value);

            if (model.OrderID.HasValue)
                query = query.Where(t => t.Transaction.OrderID == model.OrderID.Value);

            if (model.InvoiceNumber.HasValue)
                query = query.Where(t => t.Transaction.InvoiceNumber == model.InvoiceNumber.Value);

            if (model.Year.HasValue)
                query = query.Where(t => t.Transaction.Date.Year == model.Year.Value);

            if (model.Quarter.HasValue)
                query = query.Where(t => SqlFunctions.DatePart("quarter", t.Transaction.Date) == model.Quarter);

            if (model.Month.HasValue)
                query = query.Where(t => t.Transaction.Date.Month == model.Month.Value);

            if (model.Method != null)
                query = query.Where(t => t.Transaction.Method == model.Method);

            if (model.ChequeNumber.HasValue)
                query = query.Where(t => t.Transaction.ChequeNumber.Value == model.ChequeNumber.Value);

            if (model.Category != null)
                query = query.Where(t => t.Transaction.Category == model.Category);

            if (model.Payee != null)
                query = query.Where(t => t.Transaction.Payee == model.Payee);

            if (model.Budget.HasValue)
                query = query.Where(t => t.Transaction.Budget == model.Budget.Value);

            query = query.Include(t => t.Transaction).Include(t => t.Account);

            model.AccountTransactions = query.Take(100).ToList();

            IndexTotals(model);

            ViewBag.AccountID = GetAccounts(model.AccountID);
            ViewBag.OrderID = GetOrders(model.OrderID);
            ViewBag.Budget = GetBudget(model.Budget);

            return View(model);
        }

        void IndexTotals(TransactionListModel model)
        {
            ViewBag.SumDebit = model.AccountTransactions.Sum(t => t.Debit);
            ViewBag.SumCredit = model.AccountTransactions.Sum(t => t.Credit);
            ViewBag.SumTotalExTax = model.AccountTransactions.Sum(t => t.TotalExTax);
            ViewBag.SumTotalTax = model.AccountTransactions.Sum(t => t.TotalTax);
            ViewBag.SumTotal = model.AccountTransactions.Sum(t => t.Total);
        }

        //
        // GET: /Transaction/Details/5

        public ActionResult Details(int id = 0)
        {
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // GET: /Transaction/Create

        public ActionResult Create()
        {
            var model = new TransactionCreateEditModel();
            SelectLists(model);
            PostToAction("Create");
            return View(model);
        }

        

        // POST: /Transaction/Create
        [HttpPost]
        public ActionResult Create(TransactionCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                var trans = new Transaction() {
                    AccountTransactions = new List<AccountTransaction>()
                };
                MapToTransaction(model, trans);
                transLogic.CreateTransaction(trans);
                return RedirectToAction("Create");
            }
            
            SelectLists(model);
            PostToAction("Create");
            return View(model);
        }

        void PostToAction(string action)
        {
            ViewBag.PostToAction = action;
        }

        
        public ActionResult CreateFromStatementLine(int id)
        {
            var line = db.BankStatementLine.Single(bsl => bsl.ID == id);
            var trans = transLogic.GetTransactionFromBankStatementLine(line);
            var model = MapToModel(trans);
            SelectLists(model);
            PostToAction("CreateFromStatementLine");
            return View("Create", model);
        }

        // POST: /Transaction/Create
        [HttpPost]
        public ActionResult CreateFromStatementLine(int id, TransactionCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                var trans = new Transaction()
                {
                    AccountTransactions = new List<AccountTransaction>()
                };
                MapToTransaction(model, trans);

                transLogic.CreateTransaction(trans);

                var line = db.BankStatementLine.Single(bsl => bsl.ID == id);

                // Auto-recon
                if (trans.AccountTransactions.Count == 1)
                {
                    db.BankReconciliation.Add(new BankReconciliation() {
                        BankStatementLineID = id,
                        AccountTransactionID = trans.AccountTransactions.First().ID 
                    });
                    db.SaveChanges();
                }

                return RedirectToAction("Reconcile", "BankStatement", new { id = line.BankStatementID }); // Continue reconciling
            }

            SelectLists(model);
            PostToAction("CreateFromStatementLine");
            return View(model);
        }


        //
        // GET: /Transaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Transaction trans = db.Transaction.Find(id);
            
            if (trans == null)
            {
                return HttpNotFound();
            }

            var model = MapToModel(trans);

            SelectLists(model);
            PostToAction("Edit");
            return View("Create", model);
        }

        //
        // POST: /Transaction/Edit/5

        [HttpPost]
        public ActionResult Edit(TransactionCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                Transaction trans = db.Transaction.Find(model.ID);
                MapToTransaction(model, trans);
                db.SaveChanges();
                transLogic.CalculateBalances(trans);
                return RedirectToAction("Index");
            }
            SelectLists(model);
            PostToAction("Edit");
            return View("Create", model);
        }

        public ActionResult EditForAccountTransaction(int id)
        {
            var at = db.AccountTransaction.Find(id);
            return RedirectToAction("Edit", new { id = at.TransactionID });
        }

        void SelectLists(TransactionCreateEditModel model)
        {
            model.DebitAccountList = GetAccounts(model.DebitAccountID);
            model.CreditAccountList = GetAccounts(model.CreditAccountID);
            model.OrderList = GetOrders(model.OrderID);
        }

        IEnumerable<SelectListItem> GetAccounts(int? accountID)
        {
            return db.Account.OrderBy(a => a.Name).AsEnumerable().Select(a => new SelectListItem() { Value = a.ID.ToString(), Text = a.Name, Selected = (a.ID == accountID) }).AsEnumerable();
        }

        IEnumerable<SelectListItem> GetOrders(int? orderID)
        {
            return db.Order.OrderByDescending(o => o.Start).AsEnumerable().Select(o =>
                    new SelectListItem
                    {
                        Value = o.ID.ToString(),
                        Text = string.Format("{0:dd MMM yyyy} - {1} - {2} - {3}d - {4:c}", o.Start, o.EndClient, o.Title, o.Days, o.Total),
                        Selected = (o.ID == orderID)
                    }).AsEnumerable();
        }

        IEnumerable<SelectListItem> GetBudget(bool? budget)
        {
            var res = new List<SelectListItem>();
            res.Add(new SelectListItem() { Value = "true", Text = "Yes", Selected = (budget ?? false) });
            res.Add(new SelectListItem() { Value = "false", Text = "No", Selected = (!budget ?? false) });
            return res;
        }

        TransactionCreateEditModel MapToModel(Transaction trans)
        {
            var model = new TransactionCreateEditModel();

            Mapper.CreateMap<Transaction, TransactionCreateEditModel>();
            Mapper.CreateMap<AccountTransaction, TransactionCreateEditModel>();

            Mapper.Map<Transaction, TransactionCreateEditModel>(trans, model);

            var credit = trans.AccountTransactions.SingleOrDefault(t => t.Total >= 0);
            var debit = trans.AccountTransactions.SingleOrDefault(t => t.Total < 0);

            if (debit != null)
            {
                model.DebitAccountID = debit.AccountID;
                model.DebitBanked = debit.Banked;
                Mapper.Map<AccountTransaction, TransactionCreateEditModel>(debit, model);
            }
            if (credit != null)
            {
                model.CreditAccountID = credit.AccountID;
                model.CreditBanked = credit.Banked;
                Mapper.Map<AccountTransaction, TransactionCreateEditModel>(credit, model);
            }
            else
            {
                InvertTotals(debit); // Total should always appear positive, even if there is no credit account.
                Mapper.Map<AccountTransaction, TransactionCreateEditModel>(debit, model);
            }

            return model;
        }
        
        void MapToTransaction(TransactionCreateEditModel model, Transaction trans)
        {
            Mapper.CreateMap<TransactionCreateEditModel, Transaction>();
            Mapper.CreateMap<TransactionCreateEditModel, AccountTransaction>()
                  .ForMember(dest => dest.ID, opt => opt.Ignore()); // Let's not get confused between TransactionID and AccountTransactionID.

            Mapper.Map<TransactionCreateEditModel, Transaction>(model, trans);

            if (model.DebitAccountID.HasValue)
            {
                MapDebitAccount(model, trans);
            }
            if (model.CreditAccountID.HasValue)
            {
                MapCreditAccount(model, trans);
            }

            if (model.Repeat)
                CreateRepeatingTransactions(model, trans);
        }

        private void MapCreditAccount(TransactionCreateEditModel model, Transaction trans)
        {
            var credit = trans.AccountTransactions.SingleOrDefault(at => at.AccountID == model.CreditAccountID.Value);
            if (credit == null)
            {
                credit = new AccountTransaction();
                trans.AccountTransactions.Add(credit);
            }
            Mapper.Map<TransactionCreateEditModel, AccountTransaction>(model, credit);
            credit.AccountID = model.CreditAccountID.Value;
            credit.Banked = model.CreditBanked;
            CreditDebitFromTotal(credit);
        }

        private void MapDebitAccount(TransactionCreateEditModel model, Transaction trans)
        {
            var debit = trans.AccountTransactions.SingleOrDefault(at => at.AccountID == model.DebitAccountID.Value);
            if (debit == null)
            {
                debit = new AccountTransaction();
                trans.AccountTransactions.Add(debit);
            }
            Mapper.Map<TransactionCreateEditModel, AccountTransaction>(model, debit);
            debit.AccountID = model.DebitAccountID.Value;
            debit.Banked = model.DebitBanked;
            InvertTotals(debit);
            CreditDebitFromTotal(debit);
        }

        void InvertTotals(AccountTransaction trans)
        {
            trans.Total *= -1;
            trans.TotalExTax *= -1;
            trans.TotalTax *= -1;
        }

        void CreditDebitFromTotal(AccountTransaction trans)
        {
            // Warning, sucky code...
            trans.CreditExTax = CreditFromTotal(trans.TotalExTax);
            trans.CreditTax = CreditFromTotal(trans.TotalTax);
            trans.Credit = CreditFromTotal(trans.Total);
            trans.ForeignCredit = CreditFromTotal(trans.ForeignTotal);
            trans.DebitExTax = DebitFromTotal(trans.TotalExTax);
            trans.DebitTax = DebitFromTotal(trans.TotalTax);
            trans.Debit = DebitFromTotal(trans.Total);
            trans.ForeignDebit = DebitFromTotal(trans.ForeignTotal);
        }

        decimal CreditFromTotal(decimal? total)
        {
            if (!total.HasValue) return 0;
            return total.Value > 0 ? total.Value : 0;
        }

        decimal DebitFromTotal(decimal? total)
        {
            if (!total.HasValue) return 0;
            return total.Value < 0 ? total.Value * -1 : 0;
        }

        void CreateRepeatingTransactions(TransactionCreateEditModel model, Transaction trans)
        {
            CreateRepeatingMaps();
            var date = model.Date.Value;
            var previous = trans;
            while (true)
            {
                date = IncreaseDateByRepeatInterval(date, model.RepeatInterval.Value, model.RepeatFrequency.Value);
                if (date > model.RepeatUntil.Value) break;
                var next = GetNextRepeatingTransaction(previous);
                MapPreviousToNextTransaction(previous, next);
                next.Date = date;
                previous = next;
            }
            DeleteFutureTransactionsInChain(previous.NextTransactions);
            BreakLinkWithPreviousIfRepeatUntilChanged(trans);
        }

        void CreateRepeatingMaps()
        {
            Mapper.CreateMap<Transaction, Transaction>()
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Date, opt => opt.Ignore())
                  .ForMember(dest => dest.PreviousTransactionID, opt => opt.Ignore())
                  .ForMember(dest => dest.PreviousTransaction, opt => opt.Ignore())
                  .ForMember(dest => dest.NextTransactions, opt => opt.Ignore())
                  .ForMember(dest => dest.Order, opt => opt.Ignore())
                  .ForMember(dest => dest.AccountTransactions, opt => opt.Ignore());

            Mapper.CreateMap<AccountTransaction, AccountTransaction>()
                  .ForMember(dest => dest.ID, opt => opt.Ignore())
                  .ForMember(dest => dest.Banked, opt => opt.Ignore())
                  .ForMember(dest => dest.TransactionID, opt => opt.Ignore())
                  .ForMember(dest => dest.Transaction, opt => opt.Ignore())
                  .ForMember(dest => dest.Account, opt => opt.Ignore())
                  .ForMember(dest => dest.BankReconciliations, opt => opt.Ignore());
        }

        DateTime IncreaseDateByRepeatInterval(DateTime date, RepeatInterval interval, int frequency)
        {
            switch (interval)
            {
                case RepeatInterval.Month:
                    return date.AddMonths(frequency);
                case RepeatInterval.Quarter:
                    return date.AddMonths(frequency * 3);
                case RepeatInterval.Semester:
                    return date.AddMonths(frequency * 6);
                case RepeatInterval.Year:
                    return date.AddYears(frequency);
                default:
                    throw new ApplicationException(string.Format("Unsupported repeat interval encountered : {0}", Enum.GetName(typeof(RepeatInterval), interval)));
            }
        }

        Transaction GetNextRepeatingTransaction(Transaction previous)
        {
            if (previous.NextTransactions == null) 
                previous.NextTransactions = new List<Transaction>();
            if (previous.NextTransactions.Count == 0)
            {
                var next = new Transaction() { AccountTransactions = new List<AccountTransaction>() };
                foreach (var at in previous.AccountTransactions)
                    next.AccountTransactions.Add(new AccountTransaction() { AccountID = at.AccountID });
                previous.NextTransactions.Add(next);
            }
            return previous.NextTransactions.First();
        }

        void MapPreviousToNextTransaction(Transaction previous, Transaction next)
        {
            Mapper.Map<Transaction, Transaction>(previous, next);
            MapPreviousToNextAccountTransaction(previous, next);
        }

        void MapPreviousToNextAccountTransaction(Transaction previous, Transaction next)
        {
            foreach (var prevat in previous.AccountTransactions)
            {
                var nextat = next.AccountTransactions.Single(at => at.AccountID == prevat.AccountID);
                Mapper.Map<AccountTransaction, AccountTransaction>(prevat, nextat);
            }
        }

        void DeleteFutureTransactionsInChain(ICollection<Transaction> nextTransactions)
        {
            if (nextTransactions == null) return;
            if (nextTransactions.Count == 0) return;
            var next = nextTransactions.First();
            DeleteFutureTransactionsInChain(next.NextTransactions);
            db.Transaction.Remove(next);
        }

        private void BreakLinkWithPreviousIfRepeatUntilChanged(Transaction trans)
        {
            if (!trans.PreviousTransactionID.HasValue) return;
            if (trans.PreviousTransaction.RepeatUntil == trans.RepeatUntil) return;
            UpdateRepeatUntilBackwards(trans.PreviousTransaction, trans.PreviousTransaction.Date);
            trans.PreviousTransactionID = new int?();
        }

        

        //
        // GET: /Transaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Transaction trans = db.Transaction.Find(id);
            if (trans == null)
            {
                return HttpNotFound();
            }
            var model = MapToModel(trans);
            return View(model);
        }

        //
        // POST: /Transaction/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transaction.Find(id);
            if (transaction.Repeat) BreakRepeatingLink(transaction);
            db.Transaction.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        void BreakRepeatingLink(Transaction trans)
        {
            if (trans.PreviousTransactionID.HasValue)
                UpdateRepeatUntilBackwards(trans.PreviousTransaction, trans.PreviousTransaction.Date);
            if (trans.NextTransactions.Count > 0)
                BreakRepeatingLinkWithNext(trans.ID);
        }

        private void BreakRepeatingLinkWithNext(int transID)
        {
            db.Transaction.Single(t => t.PreviousTransactionID == transID).PreviousTransactionID = new int?();
        }

        void UpdateRepeatUntilBackwards(Transaction trans, DateTime repeatUntil)
        {
            trans.RepeatUntil = repeatUntil;
            if (trans.PreviousTransactionID.HasValue)
                UpdateRepeatUntilBackwards(trans.PreviousTransaction, repeatUntil);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult Methods(string term)
        {
            return Json(db.Transaction.Where(o => o.Method.StartsWith(term)).Select(o => o.Method).Distinct().OrderBy(o => o), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Categories(string term)
        {
            return Json(db.Transaction.Where(o => o.Category.StartsWith(term)).Select(o => o.Category).Distinct().OrderBy(o => o), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Payees(string term)
        {
            return Json(db.Transaction.Where(o => o.Payee.StartsWith(term)).Select(o => o.Payee).Distinct().OrderBy(o => o), JsonRequestBehavior.AllowGet);
        }





    }
}