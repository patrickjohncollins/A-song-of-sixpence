using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finances.Data.Banking;

namespace Finances.Data.Banking
{
    public class BankReconciliation
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int AccountTransactionID { get; set; }
        [ForeignKey("AccountTransactionID")]
        public virtual AccountTransaction AccountTransaction { get; set; }

        [Required]
        public int BankStatementLineID { get; set; }
        [ForeignKey("BankStatementLineID")]
        public virtual BankStatementLine StatementLine { get; set; }
        
    }
}
