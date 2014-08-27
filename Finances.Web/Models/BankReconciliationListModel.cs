using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finances.Web.Models
{
    public class BankReconciliationListModel
    {
        public int ID { get; set; }
        public string BankStatementLineDescription { get; set; }
        public string TransactionDescription { get; set; }
    }
}