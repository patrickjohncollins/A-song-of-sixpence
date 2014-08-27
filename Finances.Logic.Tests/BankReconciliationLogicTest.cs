using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finances.Logic;
using System.Reflection;
using Finances.Data;
using Finances.Data.Banking;
using EntityFramework.Extensions;

namespace Finances.Logic.Tests
{
    //[TestClass]
    public class BankReconciliationLogicTest
    {
        [TestMethod]
        public void AutoMatchTest()
        {
            var brl = new BankReconciliationLogic();

            var bsl = new List<BankStatementLine>();
            var trans = new List<AccountTransaction>();
            var expected = new List<BankReconciliation>();

            bsl.Add(new BankStatementLine()
            {
                TransType = "CASH",
                Date = DateTime.Parse("26/11/2012"),
                Amount = -50,
                Name = "CARTE X0912 RETRAIT DAB SG 25/11 ",
                Memo = "00H16 MONTPELLIER SAINT DENIS 009081",
                Currency = "EUR"
            });

            trans.Add(new AccountTransaction()
            {
                Debit = -50,
                Credit = 0,
                Total = -50,
                Transaction = new Transaction()
                {
                    Date = DateTime.Parse("24/11/2012"),
                    Method = "Retrait",
                    Category = "Cash"
                }
            });

            expected.Add(AddBankRecon(bsl.Last(), trans.Last()));

            bsl.Add(new BankStatementLine()
            {
                TransType = "DIRECTDEP",
                Date = DateTime.Parse("29/11/2012"),
                Amount = 2000,
                Name = "VIR RECU 0000031495334 ",
                Memo = "DE: MONSIEUR PATRICK JOHN COLLIMOTIF: Salaire REF: 0000033 001",
                Currency = "EUR"
            });

            trans.Add(new AccountTransaction()
            {
                Debit = 0,
                Credit = 2000,
                Total = 2000,
                Transaction = new Transaction()
                {
                    Date = DateTime.Parse("30/11/2012"),
                    Method = "Virement",
                    Category = "Salaire"
                }
            });

            expected.Add(AddBankRecon(bsl.Last(), trans.Last()));

            var res = brl.AutoMatch(bsl, trans);

            Assert.IsNotNull(res);
            Assert.AreEqual(expected.Count, res.Count);

            // This code is failing, because the results are being returned in reverse order.

            for (var i = 0; i < expected.Count; i++)
            {
                var exp1 = expected[i];
                var res1 = res[i];
                Assert.AreSame(exp1.StatementLine, res1.StatementLine);
                Assert.AreSame(exp1.AccountTransaction, res1.AccountTransaction);
            }

        }

        BankReconciliation AddBankRecon(BankStatementLine bsl, AccountTransaction trans)
        {
            return new BankReconciliation()
            {
                StatementLine = bsl,
                AccountTransaction = trans
            };
        }



    }
}
