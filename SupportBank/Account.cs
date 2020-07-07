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
        
        public void SortTransactions(List<Transaction> transactions)
        {
            this.ListOfTransactions = transactions.FindAll(t =>
                t.From.Equals(this.AccountHolder) || t.To.Equals(this.AccountHolder));
        }

        public void SimulateTransactions(List<Transaction> transactions)
        {
            this.SortTransactions(transactions);
            
            var outgoingTransactions = this.ListOfTransactions.Where(t => t.From.Equals(this.AccountHolder));
            var incomingTransactions = this.ListOfTransactions.Where(t => t.To.Equals(this.AccountHolder));

            this.AccountBalance -= outgoingTransactions.Select(t => t.Amount).Sum();
            this.AccountBalance += incomingTransactions.Select(t => t.Amount).Sum();
        }

        public void Print()
        {
            Console.WriteLine("Account Holder: " + this.AccountHolder);
            Console.WriteLine("Account Balance: " + this.AccountBalance);
            this.ListOfTransactions.ForEach(delegate(Transaction transaction) { transaction.Print(); });
        }
        
    }
}