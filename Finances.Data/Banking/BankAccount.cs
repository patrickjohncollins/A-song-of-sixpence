using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Finances.Data.Banking
{
    public class BankAccount
    {
        [Key]
        public virtual int ID { get; set; }
        [Required]
        public virtual string AccountID { get; set; }
        [Required]
        public virtual string AccountType { get; set; }
        public virtual string AccountKey { get; set; }
        public virtual string BankAccountType { get; set; }
        public virtual string BankID { get; set; }
        public virtual string BranchID { get; set; }
        public virtual ICollection<BankStatement> Statements { get; protected set; }
    }
}
