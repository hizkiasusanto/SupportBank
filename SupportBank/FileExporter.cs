using System;
using System.Collections.Generic;
using System.IO;
using NLog;

namespace SupportBank
{
    public class FileExporter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void ExportFile(List<Transaction> transactions, string filename)
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());

            string fileExt = Path.GetExtension(filename);

            switch (fileExt)
            {
                case ".csv":
                    new CsvFinancialWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                case ".json":
                    new JsonFinancialWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                case ".xml":
                    new XmlFinancialWriter().Write(transactions,filename);
                    Console.WriteLine($"File {filename} saved");
                    break;

                default:
                    Console.WriteLine($"File type {fileExt} is unrecognized");
                    break;
            }
        }
    }
}