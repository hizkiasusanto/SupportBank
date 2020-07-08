using System.Collections.Generic;
using System.IO;
using NLog;

namespace SupportBank
{
    public class FileParser
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
                    transactions = new CsvFinancialReader().Read(fileName);
                    break;
                
                case ".json":
                    transactions = new JsonFinancialReader().Read(fileName);
                    break;
                
                case ".xml":
                    transactions = new XmlFinancialReader().Read(fileName);
                    break;
                
                default:
                    logger.Error($"File type {fileExt} is unrecognized");
                    break;
            }

            return transactions;
        }
    }
}