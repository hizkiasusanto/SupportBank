using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NLog;
using NLog.Fluent;

namespace SupportBank
{
    public class XmlFinancialReader : IFinancialRecordReader
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string file)
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());

            var xDoc = XDocument.Load(file);

            var transactions = new List<Transaction>();

            var index = 1;
            foreach (XElement element in xDoc.Descendants("TransactionList").Elements("SupportTransaction"))
            {
                try
                {
                    transactions.Add(new Transaction(
                        DateTime.FromOADate(Double.Parse(element.Attribute("Date").Value)),
                        element.Element("Parties").Element("From").Value,
                        element.Element("Parties").Element("To").Value,
                        element.Element("Description").Value,
                        float.Parse(element.Element("Value").Value)));
                }
                catch
                {
                    logger.Warn($"Bad data found in {file}: entry {index}");
                    Console.WriteLine($"Bad data found in {file}. Please fix entry #{index}");
                }

                index++;
            }

            return transactions;
        }
    }
}