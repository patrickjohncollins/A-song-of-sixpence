using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Finances.Data.Banking
{
    public class BankStatement
    {
        [Key]
        public virtual int ID { get; set; }
        [Required]
        public virtual int BankAccountID { get; set; }
        [ForeignKey("BankAccountID")]
        public virtual BankAccount Account { get; set; }
        [Required]
        [DisplayFormat(DataFormatString="{0:d}", ApplyFormatInEditMode=true)]
        public virtual DateTime Start { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime End { get; set; }
        [Column(TypeName = "money")]
        public virtual decimal LedgerBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime LedgerBalanceDate { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public virtual decimal AvaliableBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime AvaliableBalanceDate { get; set; }
        public virtual ICollection<BankStatementLine> Lines { get; protected set; }
    }
}
