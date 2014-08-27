using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OFXSharp;
using System.IO;
using Finances.Data;
using Finances.Data.Banking;

namespace Finances.Logic
{
    public class OfxImporter
    {

        Entities entities = Entities.GetContext();

        public BankStatement[] ImportOfx(OFXDocument[] ofxDocuments)
        {
            var statements = new List<BankStatement>();

            foreach (var ofxDocument in ofxDocuments)
                statements.Add(ImportOfxBankStatement(ofxDocument));

            entities.SaveChanges();

            return statements.ToArray();
        }

        BankStatement ImportOfxBankStatement(OFXDocument ofxDocument)
        {
            var account = GetBankAccount(ofxDocument);
            var statement = AddStatement(ofxDocument, account);
            foreach (var transaction in ofxDocument.Transactions)
                AddStatementLine(transaction, statement);
            return statement;
        }

        BankAccount GetBankAccount(OFXDocument ofxDocument)
        {
            var account = entities.BankAccount.Where(ba => ba.AccountID == ofxDocument.Account.AccountID).FirstOrDefault();
            if (account == null)
            {
                account = entities.BankAccount.Add(entities.BankAccount.Create());
                account.AccountID = ofxDocument.Account.AccountID;
                account.AccountKey = ofxDocument.Account.AccountKey;
                account.AccountType = Enum.GetName(typeof(AccountType), ofxDocument.Account.AccountType);
                account.BankAccountType = Enum.GetName(typeof(BankAccountType), ofxDocument.Account.BankAccountType);
                account.BankID = ofxDocument.Account.BankID;
                account.BranchID = ofxDocument.Account.BranchID;
            }
            return account;
        }

        BankStatement AddStatement(OFXDocument ofxDocument, BankAccount account)
        {
            var statement = entities.BankStatement.Create();
            statement.AvaliableBalance = ofxDocument.Balance.AvaliableBalance;
            statement.AvaliableBalanceDate = ofxDocument.Balance.AvaliableBalanceDate;
            statement.LedgerBalance = ofxDocument.Balance.LedgerBalance;
            statement.LedgerBalanceDate = ofxDocument.Balance.LedgerBalanceDate;
            statement.Start = ofxDocument.StatementStart;
            statement.End = ofxDocument.StatementEnd;
            account.Statements.Add(statement);
            return statement;
        }

        BankStatementLine AddStatementLine(OFXSharp.Transaction transaction, BankStatement statement)
        {
            var line = entities.BankStatementLine.Create();
            line.Amount = transaction.Amount;
            line.CheckNum = transaction.CheckNum;
            line.Currency = transaction.Currency;
            line.Date = transaction.Date;
            //transaction.FundAvaliabilityDate;
            line.IncorrectTransactionID = transaction.IncorrectTransactionID;
            line.Memo = transaction.Memo;
            line.Name = transaction.Name;
            line.PayeeID = transaction.PayeeID;
            line.ReferenceNumber = transaction.ReferenceNumber;
            line.ServerTransactionID = transaction.ServerTransactionID;
            line.Sic = transaction.Sic;
            line.TransactionCorrectionAction = Enum.GetName(typeof(TransactionCorrectionType), transaction.TransactionCorrectionAction);
            line.TransactionID = transaction.TransactionID;
            //transaction.TransactionInitializationDate;
            //transaction.TransactionSenderAccount;
            line.TransType = Enum.GetName(typeof(OFXTransactionType), transaction.TransType);
            line.Preexisting = IsTransactionPreexisting(statement.BankAccountID, line);
            statement.Lines.Add(line);
            return line;
        }

        bool IsTransactionPreexisting(int BankAccountID, BankStatementLine line)
        {
            if (entities.BankStatementLine.Any(l => l.Statement.BankAccountID == BankAccountID && l.TransactionID == line.TransactionID))
                return true;

            if (entities.BankStatementLine.Any(l => l.Statement.BankAccountID == BankAccountID && l.Date == line.Date && l.Amount == l.Amount && l.Name == l.Name))
                return true;
            
            return false;
        }

    }
}

