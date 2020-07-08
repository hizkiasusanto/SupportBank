using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CsvHelper;
using Newtonsoft.Json;
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
            
            InitializeLogger(path);

            try
            {
                logger.Info("Running main method");

                var file = Path.Combine(path, @"Transactions2014.csv");

                var transactions = ParseFileToTransactions(file);

                logger.Info("File parsed to list of transactions successfully");

                while (true)
                {
                    Console.Write("\nEnter a command: ");

                    var inputCommand = Console.ReadLine();

                    PrintQueryCommandResult(inputCommand, transactions);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private static void InitializeLogger(string path)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = Path.Combine(path, @"Logs\SupportBank.log"),
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        private static void PrintQueryCommandResult(string inputCommand, List<Transaction> transactions)
        {
            if (inputCommand.Equals("List All"))
            {
                var accountsList = ListAll(transactions);
                accountsList.ForEach(delegate(Account account) { account.Print(); });
            }

            else if (inputCommand.StartsWith("List "))
            {
                string name = inputCommand.Substring(5);
                var account = ListOne(name, transactions);
                if (account.ListOfTransactions.Count != 0)
                {
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

        private static List<Transaction> ParseFileToTransactions(string file)
        {
            

            List<Transaction> transactions = new List<Transaction>();

            if (file.EndsWith(".csv"))
            {
                ParseCsvToTransactions(file, transactions);
            }

            else if (file.EndsWith(".json"))
            {
                ParseJsonToTransactions(file, transactions);
            }

            else if (file.EndsWith(".xml"))
            {
                ParseXmlToTransactions(file, transactions);
            }

            return transactions;
        }

        private static void ParseXmlToTransactions(string file, List<Transaction> transactions)
        {
            StreamReader reader = new StreamReader(file);
            string xml = reader.ReadToEnd();
            XDocument xDoc = XDocument.Parse(xml);
            
            List<Transaction> tempTransactions = new List<Transaction>();

            try
            {
                tempTransactions = (from element in xDoc.Descendants("TransactionList").Elements("SupportTransaction")
                        select new Transaction(
                            DateTime.FromOADate(Double.Parse(element.Attribute("Date").Value)),
                            element.Element("Parties").Element("From").Value,
                            element.Element("Parties").Element("To").Value,
                            element.Element("Description").Value,
                            float.Parse(element.Element("Value").Value))
                    ).ToList();
            }
            catch
            {
                logger.Error("Error parsing XML file to transactions: " + file);
            }
            
            transactions.AddRange(tempTransactions);
        }

        private static void ParseJsonToTransactions(string file, List<Transaction> transactions)
        {
            StreamReader reader = new StreamReader(file);
            
            List<Transaction> tempTransactions = new List<Transaction>();
            try
            {
                string json = reader.ReadToEnd();
                tempTransactions = JsonConvert.DeserializeObject<List<Transaction>>(json);
            }
            catch
            {
                logger.Error("Error parsing JSON file to transactions: " + file);
            }

            transactions.AddRange(tempTransactions);
        }

        private static void ParseCsvToTransactions(string file, List<Transaction> transactions)
        {
            StreamReader reader = new StreamReader(file);
            var csv = new CsvReader(reader, new CultureInfo("en-GB"));

            while (csv.Read())
            {
                try
                {
                    transactions.Add(csv.GetRecord<Transaction>());
                }

                catch
                {
                    logger.Error("Bad data found in " 
                                + file + ". Please fix entry in row " 
                                + csv.Parser.Context.Row + ":" + csv.Parser.Context.RawRecord);
                    Console.Write("Bad data found in " 
                                  + file + ". Please fix entry in row " 
                                  + csv.Parser.Context.Row + ":" + csv.Parser.Context.RawRecord);
                }
            }
        }

        static List<Account> ListAll(List<Transaction> transactionsList)
        {
            var uniqueNames = transactionsList.Select(t => t.From).Concat(transactionsList.Select(t => t.To))
                .Distinct();

            var accounts = uniqueNames.Select(t => new Account(t)).ToList();

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