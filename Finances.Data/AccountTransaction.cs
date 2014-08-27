using Finances.Data.Banking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Data
{
    public class AccountTransaction
    {

        [Key]
        public int ID { get; set; }

        public int TransactionID { get; set; }
        
        [ForeignKey("TransactionID")]
        public virtual Transaction Transaction { get; set; }

        [Required]
        public int AccountID { get; set; }

        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Banked { get; set; }

        [DataType(DataType.Currency)]
        public decimal? DebitExTax { get; set; }

        [DataType(DataType.Currency)]
        public decimal? DebitTax { get; set; }

        [DataType(DataType.Currency)]
        public decimal Debit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? CreditExTax { get; set; }

        [DataType(DataType.Currency)]
        public decimal? CreditTax { get; set; }

        [DataType(DataType.Currency)]
        public decimal Credit { get; set; }

        [DisplayName("Ex Tax")]
        [DataType(DataType.Currency)]
        public decimal? TotalExTax { get; set; }

        [DisplayName("Tax")]
        [DataType(DataType.Currency)]
        public decimal? TotalTax { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [DataType(DataType.Currency)]
        public decimal? ForeignDebit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? ForeignCredit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? ForeignTotal { get; set; }

        public virtual ICollection<BankReconciliation> BankReconciliations { get; protected set; }

    }
}
