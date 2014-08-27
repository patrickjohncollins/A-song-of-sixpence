using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finances.Web.Models
{
    public class BankReconciliationCreateModel : IValidatableObject
    {
        public int[] AccountTransactionID { get; set; }
        public IEnumerable<SelectListItem> AccountTransactionList { get; set; }
        public int[] BankStatementLineID { get; set; }
        public IEnumerable<SelectListItem> BankStatementLineList { get; set; }
        public string Method { get; set; }
        public IEnumerable<SelectListItem> MethodList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var memberName = context.MemberName;
            var validationErrors = new List<ValidationResult>();

            if (AccountTransactionID != null && BankStatementLineID != null)
                if (AccountTransactionID.Length > 1 && BankStatementLineID.Length > 1)
                    validationErrors.Add(new ValidationResult("Multiple statement lines and transaction are not allowed simultaneously.", new string[] { "BankStatementLineID" }));

            return validationErrors;
        }
        
    }
}