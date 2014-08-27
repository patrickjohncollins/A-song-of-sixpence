using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Finances.Data.Banking;
using System.Web.Mvc;
using Finances.Data;

namespace Finances.Web.Models
{
    public class TransactionCreateEditModel : IValidatableObject
    {
        public int ID { get; set; }
        
        [DisplayName("Debit Account")]
        public int? DebitAccountID { get; set; }

        public IEnumerable<SelectListItem> DebitAccountList { get; set; }

        [DisplayName("Credit Account")]
        public int? CreditAccountID { get; set; }

        public IEnumerable<SelectListItem> CreditAccountList { get; set; }

        [DisplayName("Order")]
        public int? OrderID { get; set; }

        public IEnumerable<SelectListItem> OrderList { get; set; }

        [DisplayName("Invoice Number")]
        public int? InvoiceNumber { get; set; }
        
        [DisplayName("Invoice Date")]
        [DataType(DataType.Date)]
        public DateTime? InvoiceDate { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        
        [DisplayName("Banked (debit)")]
        [DataType(DataType.Date)]
        public DateTime? DebitBanked { get; set; }

        [DisplayName("Banked (credit)")]
        [DataType(DataType.Date)]
        public DateTime? CreditBanked { get; set; }

        [StringLength(255)]
        public string Method { get; set; }
        
        [DisplayName("Cheque Number")]
        public int? ChequeNumber { get; set; }
        
        [StringLength(255)]
        public string Category { get; set; }
        
        [StringLength(255)]
        public string Payee { get; set; }
        
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string Nature { get; set; }

        [DisplayName("Ex Tax")]
        [DataType(DataType.Currency)]
        public decimal? TotalExTax { get; set; }

        [DisplayName("Tax")]
        [DataType(DataType.Currency)]
        public decimal? TotalTax { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? Total { get; set; }
        
        [Required]
        public bool Budget { get; set; }
        
        [Required]
        public bool Foreign { get; set; }
        
        [DataType(DataType.Currency)]
        [DisplayName("Foreign Amount")]
        public decimal? ForeignTotal { get; set; }
        
        [StringLength(3)]
        [DisplayName("Foreign Currency")]
        public string ForeignCurrency { get; set; }
        
        [DisplayName("Exchange Rate")]
        public decimal? ExchangeRate { get; set; }

        public bool Repeat { get; set; }
        public int? RepeatFrequency { get; set; }
        public RepeatInterval? RepeatInterval { get; set; }

        public IEnumerable<SelectListItem> RepeatIntervalList 
        { 
            get
            {
                var values = from Finances.Data.RepeatInterval e in Enum.GetValues(typeof(Finances.Data.RepeatInterval))
                             select new { Id = e, Name = e.ToString() };
                return new SelectList(values, "Id", "Name", RepeatInterval);
            }
        }

        [DataType(DataType.Date)]
        public DateTime? RepeatUntil { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var memberName = context.MemberName;
            var validationErrors = new List<ValidationResult>();

            if (!DebitAccountID.HasValue && !CreditAccountID.HasValue)
                validationErrors.Add(new ValidationResult("An account to debit or credit is required."));

            if (Total <= 0)
                validationErrors.Add(new ValidationResult("A positive amount is required.", new string[] { "Total" } ));

            if (Foreign)
            {
                if (!ForeignTotal.HasValue)
                    validationErrors.Add(new ValidationResult("Foreign Amount is required.", new string[] { "ForeignAmount" }));
                if (string.IsNullOrWhiteSpace(ForeignCurrency))
                    validationErrors.Add(new ValidationResult("Foreign Currency is required.", new string[] { "ForeignCurrency" }));
                if (!ExchangeRate.HasValue)
                    validationErrors.Add(new ValidationResult("Exchange Rate is required.", new string[] { "ExchangeRate" }));
            }
            if (Repeat)
            {
                if (!RepeatFrequency.HasValue)
                    validationErrors.Add(new ValidationResult("Repeat Frequency is required.", new string[] { "RepeatFrequency" }));
                if (!RepeatInterval.HasValue)
                    validationErrors.Add(new ValidationResult("Repeat Interval is required.", new string[] { "RepeatInterval" }));
                if (!RepeatUntil.HasValue)
                    validationErrors.Add(new ValidationResult("Repeat Until is required.", new string[] { "RepeatUntil" }));
                else if (RepeatUntil.Value <= Date.Value)
                    validationErrors.Add(new ValidationResult("Repeat Until must be later than transaction Date.", new string[] { "RepeatUntil" }));

            }
            return validationErrors;
        }

    }
}
