using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using NLog;

namespace SupportBank
{
    public class CsvFinancialReaderWriter : IFinancialRecordReaderWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string file)
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());

            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, new CultureInfo("en-GB"));

            var transactions = new List<Transaction>();

            while (csv.Read())
            {
                try
                {
                    transactions.Add(csv.GetRecord<Transaction>());
                }

                catch
                {
                    logger.Warn($"Bad data found in {file}: entry {csv.Parser.Context.Row}");
                    Console.WriteLine($"Bad data found in {file}. Please fix entry #{csv.Parser.Context.Row}");
                }
            }

            return transactions;
        }
        
        public void Write(List<Transaction> transactions, string file)
        {
            var csvWriter = new StreamWriter(file);
            new CsvWriter(csvWriter, new CultureInfo("en-GB")).WriteRecords(transactions);
        }
    }
}