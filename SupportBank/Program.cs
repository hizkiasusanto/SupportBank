using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            var path = Directory.GetCurrentDirectory();
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = Path.Combine(path, @"Logs\SupportBank.log"),
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            try
            {
                logger.Info("Running main method");

                var files = Directory.EnumerateFiles(path, "*.csv");
                var file = files.First();

                var transactions = ParseFileToTransactions(file);
                
                logger.Info("Ran successfully");

                while (true)
                {
                    Console.Write("\nEnter a command: ");

                    var inputCommand = Console.ReadLine();

                    PrintQueryCommandResult(inputCommand, transactions);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        }

        private static void PrintQueryCommandResult(string inputCommand, List<Transaction> transactions)
        {
            var distinctNames = transactions
                .Select(t => t.From)
                .Concat(transactions
                    .Select(t => t.To))
                .Distinct()
                .ToList();
            
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

        public static List<Transaction> ParseFileToTransactions(string file)
        {
            StreamReader reader = new StreamReader(file);
            var csv = new CsvReader(reader, new CultureInfo("en-GB"));

            List<Transaction> transactions = new List<Transaction>();
            
            while (csv.Read())
            {
                try
                {
                    transactions.Add(csv.GetRecord<Transaction>());
                }

                catch (Exception ex)
                {
                    logger.Warn("Bad data found. Please fix this entry:" + csv.Parser.Context.RawRecord);
                    Console.Write("Bad data found. Please fix this entry:");
                    Console.Write(csv.Parser.Context.RawRecord);
                }
            }
            
            return transactions;
        }

        static List<Account> ListAll(List<Transaction> transactionsList)
        {
            var uniqueNames = transactionsList.Select(t => t.From).Concat(transactionsList.Select(t => t.To))
                .Distinct();

            var accounts = uniqueNames.Select(t => new Account(t, 0)).ToList();

            foreach (Account account in accounts)
            {
                account.GetAccountBalance(transactionsList);
            }

            return accounts;
        }

        static Account ListOne(string accountHolder, List<Transaction> transactionsList)
        {
            Account accResult = new Account(accountHolder);

            accResult.FilterTransactions(transactionsList);

            return accResult;
        }
    }
}