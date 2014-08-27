using Finances.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Finances.Web.Models
{
    public class TransactionListModel
    {
        [DisplayName("Account")]
        public int? AccountID { get; set; }
        [DisplayName("Order")]
        public int? OrderID { get; set; }
        [DisplayName("Invoice Number")]
        public int? InvoiceNumber { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }
        public int? Month { get; set; }
        public string Method { get; set; }
        [DisplayName("Cheque Number")]
        public int? ChequeNumber { get; set; }
        public string Category { get; set; }
        public string Payee { get; set; }
        public bool? Budget { get; set; }

        public List<AccountTransaction> AccountTransactions { get; set; }
    }

}