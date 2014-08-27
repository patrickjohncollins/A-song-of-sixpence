using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OFXSharp;
using QifApi;
using System.IO;

namespace Finances.Logic.Qif
{
    public class QifToOfx
    {
        public OFXDocument Convert(Stream qifFile)
        {
            using (var sr = new StreamReader(qifFile)) 
            {
                return Convert(sr);
            }
        }

        public OFXDocument Convert(StreamReader sr)
        {
            return Convert(QifDom.ImportFile(sr));
        }

        public OFXDocument Convert(QifDom qifDom)
        {
            var ofx = new OFXDocument();
            
            // TODO : This is really cheating, I should be asking for the associated bank account when uploading...
            ofx.Account = new Account() { AccountID = "00040621193" };
            ofx.Balance = new Balance() { AvaliableBalance = 0, AvaliableBalanceDate = DateTime.Now, LedgerBalance = 0, LedgerBalanceDate = DateTime.Now };
            ofx.StatementStart = DateTime.Now;
            ofx.StatementEnd = DateTime.Now;
            
            ofx.Transactions = (from bt in qifDom.BankTransactions
                                select new Transaction() {
                                    Amount = bt.Amount,
                                    Date = bt.Date,
                                    Name = bt.Payee,
                                    TransactionID = Guid.NewGuid().ToString("D")
                                }).ToList();
            return ofx;
        }
    }
}
