using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Data.Banking
{
    public class BankStatementLine
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int BankStatementID { get; set; }
        [ForeignKey("BankStatementID")]
        public virtual BankStatement Statement { get; set; }
        [Required]
        public string TransType { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        [Column(TypeName="money")]
        public decimal Amount { get; set; }
        [Required]
        public string TransactionID { get; set; }
        [Required]
        public string Name { get; set; }

        //public DateTime TransactionInitializationDate { get; set; }

        //public DateTime FundAvaliabilityDate { get; set; }

        public string Memo { get; set; }

        public string IncorrectTransactionID { get; set; }

        public string TransactionCorrectionAction { get; set; }

        public string ServerTransactionID { get; set; }

        public string CheckNum { get; set; }

        public string ReferenceNumber { get; set; }

        public string Sic { get; set; }

        public string PayeeID { get; set; }

        //public Account TransactionSenderAccount { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// Indicates that the transaction was already imported on a prior statement.
        /// </summary>
        /// <remarks>
        /// Preexisting lines are ignored during the bank recon.
        /// </remarks>
        [Required]
        public bool Preexisting { get; set; }

        public virtual ICollection<BankReconciliation> BankReconciliations { get; protected set; }
    }
}
