using Finances.Data;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Logic
{
    public class BudgetLogic
    {

        private Entities db = Entities.GetContext();

        public void FulfillBudget(int accountID, string category, DateTime date, decimal amount)
        {
            var at = FindBudgetTransactionToFulfill(accountID, category, date);
            if (at == null) return;

            if (Math.Abs(amount) > Math.Abs(at.Total))
                at.Total = 0;
            else 
                at.Total += amount;

            db.SaveChanges();

            // Find the budget transaction 
        }

        AccountTransaction FindBudgetTransactionToFulfill(int accountID, string category, DateTime date)
        {
            return (from at in db.AccountTransaction
                    where at.AccountID == accountID
                    && at.Transaction.Category == category
                    && at.Transaction.Budget == true
                        //&& at.BudgetFulfill == true
                    && date >= at.Transaction.Date
                    //&& date <= at.BudgetFulfillUntil 
                    select at).FirstOrDefault();
        }

    }
}
