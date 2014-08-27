using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Finances.Data
{
    public class Account
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [DisplayName("Associated Bank Account")]
        public virtual string BankAccountID { get; set; }

        [DisplayName("Current")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal CurrentBalance { get; set; }

        [DisplayName("Banked")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal BankedBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; protected set; }

        [DefaultValue(false)]
        public bool Closed { get; set; }
    }
}
