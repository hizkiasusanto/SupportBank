using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SupportBank
{
    internal class Program
    {
        public static void Main()
        {
            StreamReader reader = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))), @"Transactions2014.csv")));
            var csv = new CsvReader(reader, new CultureInfo("en-GB"));

            var transactions = csv.GetRecords<Transaction>().ToList();
            
            var distinctNames = transactions.Select(t => t.From).Concat(transactions.Select(t => t.To)).Distinct().ToList();

            while (true)
            {
                Console.Write("\nEnter a command: ");

                var inputCommand = Console.ReadLine();

                if (inputCommand.Equals("List All"))
                {
                    var accountsList = ListAll(transactions);
                    accountsList.ForEach(delegate(Account account) { account.Print(); });
                }

                else if (inputCommand.StartsWith("List "))
                {
                    string name = inputCommand.Substring(5);
                    if (distinctNames.Contains(name))
                    {
                        var account = ListOne(name, transactions);
                        account.Print();
                    }
                    else
                    {
                        Console.WriteLine("Account named " + name + " doesn't exist");
                    }
                }

                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }

        static List<Account> ListAll(List<Transaction> transactionsList)
        {
            var uniqueNames = transactionsList.Select(t => t.From).Concat(transactionsList.Select(t => t.To)).Distinct();
            
            var accounts = uniqueNames.Select(t => new Account(t, 0)).ToList();
            
            foreach (Account account in accounts)
            {
                account.SortTransactions(transactionsList);
            }

            return accounts;
        }

        static Account ListOne(string accountHolder, List<Transaction> transactionsList)
        {
            Account accResult = new Account(accountHolder);
            
            accResult.SimulateTransactions(transactionsList);

            return accResult;
        }
    }
}