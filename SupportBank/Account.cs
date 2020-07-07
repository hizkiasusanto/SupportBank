using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportBank
{
    public class Account
    {
        public string AccountHolder;
        public float AccountBalance;
        public List<Transaction> ListOfTransactions;

        public Account(string name, float balance = 0)
        {
            this.AccountHolder = name;
            this.AccountBalance = balance;
        }

        public void ChangeBalance(float transaction)
        {
            this.AccountBalance += transaction;
        }
        
        public void FilterTransactions(List<Transaction> transactions)
        {
            this.ListOfTransactions = transactions.FindAll(t =>
                t.From.Equals(this.AccountHolder) || t.To.Equals(this.AccountHolder));
        }

        public void GetAccountBalance(List<Transaction> transactions)
        {
            this.FilterTransactions(transactions);
            
            var debitTransactions = this.ListOfTransactions.Where(t => t.From.Equals(this.AccountHolder));
            var creditTransactions = this.ListOfTransactions.Where(t => t.To.Equals(this.AccountHolder));

            this.AccountBalance -= debitTransactions.Select(t => t.Amount).Sum();
            this.AccountBalance += creditTransactions.Select(t => t.Amount).Sum();
        }

        public void Print()
        {
            Console.WriteLine("Account Holder: " + this.AccountHolder);
            Console.WriteLine("Account Balance: " + this.AccountBalance);
            this.ListOfTransactions.ForEach(delegate(Transaction transaction) { transaction.Print(); });
        }
        
    }
}