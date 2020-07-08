using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;

namespace SupportBank
{
    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());

            try
            {
                logger.Info("Running main method");

                var files = Directory.EnumerateFiles(@"FinancialRecords");

                var transactions = new List<Transaction>();
                foreach (var file in files)
                {
                    transactions.AddRange(FileParser.ParseFileToTransactions(file));
                }

                while (true)
                {
                    Console.Write("\nEnter a command: ");

                    var inputCommand = Console.ReadLine();

                    QueryResponder.PrintQueryCommandResult(inputCommand, transactions);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}