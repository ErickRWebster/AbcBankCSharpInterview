﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    [TestClass]
    public class BankTest
    {
        private static readonly double DOUBLE_DELTA = 1e-15;

        [TestMethod]
        public void CustomerSummary() 
        {
            Bank bank = new Bank();
            Customer john = new Customer("John");
            john.OpenAccount(new Account(Account.AccountType.CHECKING));
            bank.AddCustomer(john);

            Assert.AreEqual("Customer Summary\n - John (1 account)", bank.CustomerSummary());
        }

        [TestMethod]
        public void CheckingAccount() {
            Bank bank = new Bank();
            Account checkingAccount = new Account(Account.AccountType.CHECKING);
            Customer bill = new Customer("Bill").OpenAccount(checkingAccount);
            bank.AddCustomer(bill);

            checkingAccount.Deposit(100.00);

            Assert.AreEqual(0.10, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        [TestMethod]
        public void SavingsAccount() {
            Bank bank = new Bank();
            Account savingsAccount = new Account(Account.AccountType.SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(savingsAccount));

            savingsAccount.Deposit(1500.0);

            Assert.AreEqual(2.00, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        [TestMethod]
        public void MaxiSavingsAccount_HighInterest() {
            Bank bank = new Bank();
            Account maxiSavingsAccount = new Account(Account.AccountType.MAXI_SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(maxiSavingsAccount));

            maxiSavingsAccount.Deposit(3000.00);

            Assert.AreEqual(150.00, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        [TestMethod]
        public void MaxiSavingsAccount_LowInterest()
        {
            Bank bank = new Bank();
            Account maxiSavingsAccount = new Account(Account.AccountType.MAXI_SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(maxiSavingsAccount));

            maxiSavingsAccount.Deposit(3000.00);
            maxiSavingsAccount.Withdraw(200.00);

            Assert.AreEqual(28.00, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        [TestMethod]
        public void DailyInterestCalculationAndDeposit()
        {
            Bank bank = new Bank();
            Account checkingAccount = new Account(Account.AccountType.CHECKING);
            bank.AddCustomer(new Customer("Bill").OpenAccount(checkingAccount));

            checkingAccount.Deposit(3000.00);
            bank.DailyInterestCalculation();

            var expectedTotal = 3000 + ((.001 * 3000)/365);

            Assert.AreEqual(expectedTotal, checkingAccount.SumTransactions(), DOUBLE_DELTA);
        }
    }
}
