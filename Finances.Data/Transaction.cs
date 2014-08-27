using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Finances.Data.Banking;

namespace Finances.Data
{
    public class Transaction
    {
        [Key]
        public int ID { get; set; }

        public int? OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public int? InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [StringLength(255)]
        public string Method { get; set; }
        
        [DisplayName("Cheque Number")]
        public int? ChequeNumber { get; set; }
        
        [StringLength(255)]
        public string Category { get; set; }
        
        [StringLength(255)]
        public string Payee { get; set; }
        
        [StringLength(255)]
        public string Nature { get; set; }

        [Required]
        public bool Budget { get; set; }
        
        [Required]
        public bool Foreign { get; set; }
        
        [StringLength(3)]
        public string ForeignCurrency { get; set; }
        
        public decimal? ExchangeRate { get; set; }

        public bool Repeat { get; set; }
        
        public int? RepeatFrequency { get; set; }
        
        public RepeatInterval? RepeatInterval { get; set; }
        
        public DateTime? RepeatUntil { get; set; }

        [ForeignKey("PreviousTransaction")]
        public int? PreviousTransactionID { get; set; }

        [InverseProperty("NextTransactions")]
        [ForeignKey("PreviousTransactionID")]
        public virtual Transaction PreviousTransaction { get; set; }

        [InverseProperty("PreviousTransaction")]
        public virtual ICollection<Transaction> NextTransactions { get; set; }

        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }

    }

    public enum RepeatInterval
    {
        Month = 2,
        Quarter = 3,
        Semester = 4,
        Year = 5
    }
}
