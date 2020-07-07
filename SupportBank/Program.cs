using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SupportBank
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            StreamReader reader = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))), @"Transactions2014.csv")));
            var csv = new CsvReader(reader, new CultureInfo("en-GB"));

            var transactions = csv.GetRecords<Transaction>();
            
            var distinctNames = transactions.Select(t => t.From).Concat(transactions.Select(t => t.To)).Distinct();

            var accounts = distinctNames.Select(t => new Account(t, 0));

            foreach (var account in accounts)
            {
                Console.WriteLine(account.AccountHolder);
            }
        }
        
    }
}