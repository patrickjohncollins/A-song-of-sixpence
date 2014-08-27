using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finances.Logic;
using System.Reflection;
using Finances.Data;
using EntityFramework.Extensions;
using QifApi;
using System.IO;
using Finances.Logic.Qif;

namespace Finances.Logic.Tests
{
    //[TestClass]
    public class QifImporterTest
    {

        
        [TestInitialize()]
        public void Initialize()
        {
            //var entities = Entities.GetContext();
            //entities.BankAccount.Delete();
        }
        
        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void ImportQifStatements()
        {
            var fileName = "2013_10_a.qif";
            var inputQif = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finances.Logic.Tests." + fileName);
            var foo = new BankStatementUploadLogic();
            var ofxDocuments = foo.ProcessStatement(fileName, inputQif);

            var ofx = ofxDocuments.First().Transactions;
            Assert.AreEqual(166, ofx.Count);
            Assert.AreEqual(19, ofx.Where(t => t.TransType == OFXSharp.OFXTransactionType.CASH).Count());  
            Assert.AreEqual(8, ofx.Where(t => t.TransType == OFXSharp.OFXTransactionType.DIRECTDEP).Count());  
        }

        [TestMethod]
        public void UploadQifStatements()
        {
            var fileName = "2013_10_a.qif";
            var inputQif = Assembly.GetExecutingAssembly().GetManifestResourceStream("Finances.Logic.Tests." + fileName);
            var foo = new BankStatementUploadLogic();
            var bankStatements = foo.UploadStatements(fileName, inputQif);
            Assert.Inconclusive();
        }

    }
}
