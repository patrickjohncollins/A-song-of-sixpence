using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OFXSharp;

namespace Finances.Logic.Ofx.PostProcessing.Boursorama
{
    /// <summary>
    /// Post processing of OFX files for the French bank Boursorama.
    /// </summary>
    public class PostProcessor
    {
        OFXSharp.Transaction transaction;

        public void Process(OFXDocument[] ofxDocuments)
        {
            foreach (var ofxDocument in ofxDocuments)
            {
                foreach (var transaction in ofxDocument.Transactions)
                {
                    this.transaction = transaction;
                    ProcessTransaction();
                }
            }
        }

        /// <summary>
        /// Calls a series of functions in an attempt to detemine the transaction type.  If a true value is returned then the transaction
        /// type has been set and the function without performing further tests.
        /// </summary>
        void ProcessTransaction()
        {
            if (transaction.Amount < 0)
                transaction.TransType = OFXTransactionType.DEBIT;
            else
                transaction.TransType = OFXTransactionType.CREDIT;
            
            if (CHECK()) return;
            if (CASH()) return;
            if (DIRECTDEP()) return;
            if (DEP()) return;
            if (POS()) return;
            if (DIRECTDEBIT()) return;
            if (FEE()) return;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "CHQ. N." and if so, sets the transaction type to CHECK (Check), 
        /// and sets the check number.
        /// </summary>
        /// <example>
        /// CHQ. N.1896270 
        /// </example>
        bool CHECK()
        {
            var cheque = "CHQ. N.";
            if (transaction.Name.StartsWith(cheque))
            {
                transaction.TransType = OFXTransactionType.CHECK;
                transaction.CheckNum = transaction.Name.Replace(cheque, "").Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "RETRAIT" and if so, sets the
        /// transaction type to CASH (Cash Withdrawl).
        /// </summary>
        /// <example>
        /// RETRAIT DAB 050613 MONTPELLIER S CB*9838
        /// </example>
        bool CASH()
        {
            if (transaction.Name.StartsWith("RETRAIT"))
            {
                transaction.TransType = OFXTransactionType.CASH;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "VERSEMENT" and if so, sets the transaction type to DEP (Deposit).
        /// </summary>
        /// <example>
        /// VIR 1962119CAV-COLLINS Patrick
        /// </example>
        bool DEP()
        {
            if (transaction.Name.StartsWith("VIR"))
            {
                transaction.TransType = OFXTransactionType.DEP;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "VIR SEPA" and if so, sets the transaction type to DIRECTDEP (Direct Deposit).
        /// </summary>
        /// <example>
        /// VIR SEPA MONSIEUR PATRICK JOHN COLLINS  
        /// </example>
        bool DIRECTDEP()
        {
            if (transaction.Name.StartsWith("VIR SEPA"))
            {
                transaction.TransType = OFXTransactionType.DIRECTDEP;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "PAIEMENT CARTE" and if so, sets the transaction type to POS (Point of Sale transfer).
        /// </summary>
        /// <example>
        /// PAIEMENT CARTE 050613 GB WWW.BRITANNIA-PD
        /// </example>
        bool POS()
        {
            if (transaction.Name.StartsWith("PAIEMENT CARTE"))
            {
                transaction.TransType = OFXTransactionType.POS;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "PRLV" and if so, sets the transaction type to DIRECTDEBIT (Merchant Initiated Debit).
        /// </summary>
        /// <example>
        /// *PRLV Cotisat. Boursorama Protection 0  
        /// PRLV Cotisat. Boursorama Protection 0   
        /// </example>
        /// <remarks>
        /// There may be a leading asterisk.
        /// </remarks>
        bool DIRECTDEBIT()
        {
            var debitorder = "PRLV";
            if (transaction.Name.StartsWith(debitorder) || transaction.Name.StartsWith("*" + debitorder))
            {
                transaction.TransType = OFXTransactionType.DIRECTDEBIT;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction is a bank fee and if so, sets the transaction type to FEE.
        /// </summary>
        /// <remarks>
        /// As yet, no bank fees for Boursorama.
        /// </remarks>
        bool FEE()
        {
            return false;
        }


    }
}
