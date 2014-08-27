using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Data.Banking;

namespace Finances.Logic
{
    public class TransactionLogic
    {

        private Entities db = Entities.GetContext();

        public void CreateTransaction(Transaction transaction)
        {
            db.Transaction.Add(transaction);
            db.SaveChanges();
            //CalculateBalances(transaction);
        }

        //public void EditTransaction(Transaction transaction)
        //{
        //    //db.Entry(transaction).State = EntityState.Modified;
        //    db.SaveChanges();
        //    CalculateBalances(transaction);
        //}

        public void CalculateBalances(Transaction transaction)
        {
            foreach (var at in transaction.AccountTransactions)
                db.Database.ExecuteSqlCommand(string.Format("CalculateBalances {0}", at.AccountID));

            db.Database.ExecuteSqlCommand("CalculateBudgetTax");

            //CalculateBudgetTax(); // TODO : Little dirty to stick this in here...
        }
        /*
        void CalculateBudgetTax()
        {
            using (var ts = new TransactionScope())
            {
                foreach (var at in GetBudgetTax())
                    CalculateBudgetTax(at);
            }
        }

        IEnumerable<AccountTransaction> GetBudgetTax()
        {
            return from at in db.AccountTransaction.Include("Transaction")
                   where at.Transaction.Category == "TVA" // TODO : Hard coding strings in cheating.
                   && at.Transaction.Budget == true
                   orderby at.Transaction.Date 
                   select at;
        }

        void CalculateBudgetTax(AccountTransaction at)
        {
            at.DebitExTax = 0;
            at.DebitTax = GetTotalTaxBeforeDate(at.Transaction.Date);
            at.Debit = at.DebitTax ?? 0;

            at.TotalExTax = at.DebitExTax * -1;
            at.TotalTax = at.DebitTax * -1;
            at.Total = at.Debit * -1;

            db.SaveChanges(); // Must update DB so that SUM for next row takes these values into account.
        }

        decimal? GetTotalTaxBeforeDate(DateTime date)
        {
            return db.AccountTransaction.Where(at => at.Transaction.Date < date).Sum(at => at.TotalTax);
        }
        */

        // TODO : Need some unit tests.
        // TODO : Extra info can be gleaned from the name and memo (ex : foreign exchange rates).
        public Transaction GetTransactionFromBankStatementLine(BankStatementLine line)
        {
            var accountID = (from a in db.Account where a.BankAccountID == line.Statement.Account.AccountID select a.ID).FirstOrDefault();

            return new Transaction()
            {
                Method = ConvertStatementTransactionTypeToPaymentMethod(line.TransType),
                Date = line.Date,
                Nature = line.Name + line.Memo,
                ChequeNumber = GetChequeNumberFromStatementLine(line.CheckNum),
                AccountTransactions = new List<AccountTransaction>() 
                {
                    new AccountTransaction()
                    {
                        Credit = line.Amount > 0 ? line.Amount : 0,
                        Debit = line.Amount < 0 ? line.Amount * -1 : 0,
                        Total = line.Amount,
                        AccountID = accountID,
                        Banked = line.Date 
                    }
                }
            };

        }

        int? GetChequeNumberFromStatementLine(string checkNum)
        {
            int chequeNumber;
            if (int.TryParse(checkNum, out chequeNumber)) return chequeNumber;
            return new int?();
        }

        internal string ConvertStatementTransactionTypeToPaymentMethod(string transType)
        {
            if (transType == "CASH") return "Retrait";
            if (transType == "CHECK") return "Chèque";
            if (transType == "CREDIT") return string.Empty;
            if (transType == "DEBIT") return string.Empty;
            if (transType == "DEP") return string.Empty;
            if (transType == "DIRECTDEBIT") return "Prelevement";
            if (transType == "DIRECTDEP") return "Virement";
            if (transType == "FEE") return "Prelevement";
            if (transType == "POS") return "CB";
            return string.Empty;
        }
    }
}
