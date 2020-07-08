using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;

namespace SupportBank
{
    public class FileReaderImportExport
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public static List<Transaction> ParseFileToTransactions(string fileName)
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());
            
            List<Transaction> transactions = new List<Transaction>();

            string fileExt = Path.GetExtension(fileName);

            switch (fileExt)
            {
                case ".csv":
                    transactions = new CsvFinancialReaderWriter().Read(fileName);
                    break;
                
                case ".json":
                    transactions = new JsonFinancialReaderWriter().Read(fileName);
                    break;
                
                case ".xml":
                    transactions = new XmlFinancialReaderWriter().Read(fileName);
                    break;
                
                default:
                    logger.Error($"File type {fileExt} is unrecognized");
                    break;
            }
            return transactions;
        }
        
        public void ParseImportedFilesToTransactions(string filename, List<Transaction> transactions)
        {
            transactions.AddRange(ParseFileToTransactions(filename));
        }
        
        public void ExportFile(List<Transaction> transactions, string filename)
        {
            string fileExt = Path.GetExtension(filename);

            switch (fileExt)
            {
                case ".csv":
                    new CsvFinancialReaderWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                case ".json":
                    new JsonFinancialReaderWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                case ".xml":
                    new XmlFinancialReaderWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                default:
                    Console.WriteLine($"File type {fileExt} is unrecognized");
                    break;
            }
        }
    }
}