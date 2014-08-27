using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Data.Banking;
using Finances.Data;

namespace Finances.Logic
{
    public class BankReconciliationLogic
    {

        public decimal GetTotalForTransactions(int[] accountTransactionID)
        {
            var db = Entities.GetContext();
            return db.AccountTransaction.Where(t => accountTransactionID.Contains(t.ID)).Sum(t => t.Total);
        }

        public decimal GetTotalForStatementLines(int[] bankStatementLineID)
        {
            var db = Entities.GetContext();
            return db.BankStatementLine.Where(bsl => bankStatementLineID.Contains(bsl.ID)).Sum(bsl => bsl.Amount);
        }

        public List<BankReconciliation> AutoMatch(List<BankStatementLine> statementLines, List<AccountTransaction> transactions)
        {
            var res = new List<BankReconciliation>();

            // Man, this code is so ugly.
            // Back to basics :
            // Loop through the statement lines.
            // - Try and find an exact matching transaction.
            // - If match found, remove statement line and transaction from lists, to avoid comparing again.
            // Once end reached, if statement lines to be matched, go through a second time, this time
            // searching with a date range +/- 1 jour, and then again if needed, up to 3 days.




            var marginDays = 0;
            while (marginDays <= 3)
            {
                for (var i = statementLines.Count - 1; i >= 0; i--)
                {
                    var bsl = statementLines[i];
                    var trans = AutoMatch(bsl, transactions, marginDays);
                    if (trans != null)
                    {
                        transactions.Remove(trans);
                        statementLines.Remove(bsl);
                        i--;
                        res.Add(new BankReconciliation()
                        {
                            StatementLine = bsl,
                            AccountTransaction = trans
                        });
                    }
                }
                marginDays++;
            }

            
            
            return res;


            // Pass in a list of bank statements lines and transactions
            // Or read these from the database?
            // Perhaps in seperate method
            // Then start matching
            // Return the list of possible matches for validation by the user

        }

        AccountTransaction AutoMatch(BankStatementLine bsl, List<AccountTransaction> transactions, int marginDays)
        {
            var tl = new TransactionLogic();
            var seekMethod = tl.ConvertStatementTransactionTypeToPaymentMethod(bsl.TransType);

            var dateStart = bsl.Date.AddDays(-2);
            var dateEnd = bsl.Date.AddDays(2);

            var matches = from t in transactions
                          where t.Transaction.Method == seekMethod
                          && t.Total == bsl.Amount
                          && t.Transaction.Date >= dateStart
                          && t.Transaction.Date <= dateEnd 
                          select t;

            if (matches.Count() == 1)
            {
                return matches.First();
            }

            return null;
        }


    }
}
