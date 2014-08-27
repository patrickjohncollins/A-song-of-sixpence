using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finances.Logic;
using System.Reflection;
using Finances.Data;
using EntityFramework.Extensions;

namespace Finances.Logic.Tests
{
    //[TestClass]
    public class OfxImporterTest
    {

        
        [TestInitialize()]
        public void Initialize()
        {
            var entities = Entities.GetContext();
            entities.BankAccount.Delete();
        }
        
        [TestCleanup()]
        public void Cleanup()
        {
        }

        //[TestMethod]
        public void ImportOfxStatements()
        {
            ImportOfxStatement1();
            ImportOfxStatement2();
        }

        public void ImportOfxStatement1()
        {
            
            var inputOfx = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finances.Logic.Tests.00050007609-4.ofx");
            var foo = new BankStatementUploadLogic();
            var ofxDocuments = foo.ProcessOfxStatement(inputOfx);
            var ofxImporter = new OfxImporter();
            
            ofxImporter.ImportOfx(ofxDocuments);

            var entities = Entities.GetContext();

            VerifyBankAccounts();

            Assert.AreEqual(2, entities.BankStatement.Count(), "Incorrect number of bank statements.");

            var statements = (from bs in entities.BankStatement where bs.Account.AccountType == "BANK" select bs).ToList();

            Assert.AreEqual(1, statements.Count(), "Incorrect number of bank statements.");

            var statement = statements.First();

            Assert.AreEqual(new DateTime(2012, 08, 21), statement.Start);
            Assert.AreEqual(new DateTime(2012, 09, 26), statement.End);

            var cheques = (from l in statement.Lines where l.TransType == "CHECK" orderby l.CheckNum select l.CheckNum).ToArray();
            Assert.AreEqual(4, cheques.Count(), "Incorrect number of cheques.");
            CollectionAssert.AreEqual(cheques, new string[] { "14", "20", "21", "22" }, "Cheque numbers do not match.");


            // TODO : Validate statements

        }

        public void ImportOfxStatement2()
        {

            var inputOfx = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finances.Logic.Tests.00050007609-6.ofx");
            var foo = new BankStatementUploadLogic();
            var ofxDocuments = foo.ProcessOfxStatement(inputOfx);
            var ofxImporter = new OfxImporter();

            var entities = Entities.GetContext();

            VerifyBankAccounts();

            Assert.AreEqual(4, entities.BankStatement.Count(), "Incorrect number of bank statements.");

            var statements = (from bs in entities.BankStatement where bs.Account.AccountType == "BANK" select bs).ToList();

            Assert.AreEqual(2, statements.Count(), "Incorrect number of bank statements.");

            var statement1 = statements[0];
            var statement2 = statements[1];

            Assert.AreEqual(new DateTime(2012, 09, 06), statement2.Start);
            Assert.AreEqual(new DateTime(2012, 10, 12), statement2.End);

            var prevLines = (from l in statement1.Lines orderby l.TransactionID select l.TransactionID).ToArray();

            var existingLines = statement2.Lines.Where(l => prevLines.Contains(l.TransactionID)).ToList();
            var newLines = statement2.Lines.Where(l => !prevLines.Contains(l.TransactionID)).ToList();

            Assert.AreEqual(0, existingLines.Where(l => l.Preexisting == false).Count(), "Lines found on previous statement should be marked preexisting.");
            Assert.AreEqual(0, newLines.Where(l => l.Preexisting == true).Count(), "Lines not found on previous statement should not be marked preexisting.");

            


        }

        void VerifyBankAccounts()
        {
            var entities = Entities.GetContext();

            Assert.AreEqual(2, entities.BankAccount.Count(), "Incorrect number of bank accounts.");

            var bankAccount = entities.BankAccount.Where(ba => ba.AccountType == "BANK").FirstOrDefault();

            Assert.IsNotNull(bankAccount, "Can't find checking account.");

            Assert.AreEqual("30003", bankAccount.BankID);
            Assert.AreEqual("01428", bankAccount.BranchID);
            Assert.AreEqual("00050007609", bankAccount.AccountID);

            bankAccount = entities.BankAccount.Where(ba => ba.AccountType == "CC").FirstOrDefault();

            Assert.IsNotNull(bankAccount, "Can't find credit card account.");

            Assert.AreEqual("000004973019590570912", bankAccount.AccountID);
        }
    }
}
