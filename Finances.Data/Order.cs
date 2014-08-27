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
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Client { get; set; }
        
        [StringLength(255)]
        [DisplayName("End Client")]
        public string EndClient { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Fee { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Travel { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        
        [StringLength(255)]
        public string State { get; set; }
    }
}
