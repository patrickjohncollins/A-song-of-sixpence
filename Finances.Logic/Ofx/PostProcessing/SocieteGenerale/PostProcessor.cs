using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OFXSharp;

namespace Finances.Logic.Ofx.PostProcessing.SocieteGenerale
{
    /// <summary>
    /// Post processing of OFX files for the French bank Société Générale.
    /// </summary>
    /// <remarks>
    /// OFX files from this bank contain only DEBIT and CREDIT transactions.  This class attemps to determine the transaction type
    /// for each transaction based on textual information stored in the name.
    /// </remarks>
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
            if (CHECK()) return;
            if (CASH()) return;
            if (DEP()) return;
            if (DIRECTDEP()) return;
            if (POS()) return;
            if (DIRECTDEBIT()) return;
            if (FEE()) return;
  
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "CHEQUE" and if so, sets the transaction type to CHECK (Check), 
        /// and sets the check number.
        /// </summary>
        /// <example>
        /// CHEQUE      20
        /// </example>
        bool CHECK()
        {
            var cheque = "CHEQUE";
            if (transaction.Name.StartsWith(cheque))
            {
                transaction.TransType = OFXTransactionType.CHECK;
                transaction.CheckNum = transaction.Name.Replace(cheque, "").Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "CARTE" and contains the label "RETRAIT" and if so, sets the
        /// transaction type to CASH (Cash Withdrawl).
        /// </summary>
        /// <example>
        /// CARTE X0912 RETRAIT DAB 31/08 08
        /// </example>
        bool CASH()
        {
            if (transaction.Name.StartsWith("CARTE") && transaction.Name.Contains("RETRAIT"))
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
        /// VERSEMENT EXPRESS 3110518376
        /// </example>
        bool DEP()
        {
            if (transaction.Name.StartsWith("VERSEMENT"))
            {
                transaction.TransType = OFXTransactionType.DEP;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "VIR RECU" and if so, sets the transaction type to DIRECTDEP (Direct Deposit).
        /// </summary>
        /// <example>
        /// VIR RECU    0000021528240
        /// </example>
        bool DIRECTDEP()
        {
            if (transaction.Name.StartsWith("VIR RECU"))
            {
                transaction.TransType = OFXTransactionType.DIRECTDEP;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "CARTE" and if so, sets the transaction type to POS (Point of Sale transfer).
        /// </summary>
        /// <example>
        /// CARTE X0912 12/08 RELAIS ABRIVAD
        /// </example>
        /// <remarks>
        /// There is a potential collision here with CASH, at both types start with CARTE, which is why CASH must be tested first.
        /// </remarks>
        bool POS()
        {
            if (transaction.Name.StartsWith("CARTE"))
            {
                transaction.TransType = OFXTransactionType.POS;
                transaction.TransactionID = ""; // Ignore the transaction ID, it always resets with each import
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "PRELEVEMENT" and if so, sets the transaction type to DIRECTDEBIT (Merchant Initiated Debit).
        /// </summary>
        /// <example>
        /// PRELEVEMENT 7974083429  GDF SUEZ
        /// </example>
        bool DIRECTDEBIT()
        {
            if (transaction.Name.StartsWith("PRELEVEMENT"))
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
        /// Bank fees are not very clearly identified, the matches used in the function may not cover all possible cases.
        /// </remarks>
        /// <example>
        /// FRAIS PAIEMENT HORS ZONE EURO
        /// OPTION TRANQUILLITE - JAZZ PRO
        /// JAZZ REDUCTION JAZZ PRO -20%
        /// </example>
        bool FEE()
        {
            if (transaction.Name.StartsWith("FRAIS PAIEMENT HORS ZONE EURO") ||
                transaction.Name.StartsWith("OPTION TRANQUILLITE - JAZZ PRO") ||
                transaction.Name.StartsWith("JAZZ REDUCTION JAZZ PRO -20%"))
            {
                transaction.TransType = OFXTransactionType.FEE;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Identifies if the transaction name starts with the label "PRLV" and if so, sets the transaction type to REPEATPMT (Repeating Payment).
        /// </summary>
        /// <example>
        /// PRLV PEL 01428T1254601038
        /// </example>
        /// <remarks>
        /// There may be other types of repeating payments not covered by this filter.
        /// </remarks>
        bool REPEATPMT()
        {
            if (transaction.Name.StartsWith("PRLV"))
            {
                transaction.TransType = OFXTransactionType.REPEATPMT;
                return true;
            }
            return false;
        }

        //[Description("Interest")]
        //INT,
        //[Description("Dividend")]
        //DIV,
        //[Description("Service Charge")]
        //SRVCHG,
        //[Description("ATM transfer")]
        //ATM,
        //[Description("Transfer")]
        //XFER,
        //[Description("Payment")]
        //PAYMENT,
        //OTHER,

    }
}
