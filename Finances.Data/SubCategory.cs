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
    class SubCategory
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

    }
}
