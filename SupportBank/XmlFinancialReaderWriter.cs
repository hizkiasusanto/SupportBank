using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using NLog;

namespace SupportBank
{
    public class XmlFinancialReaderWriter : IFinancialRecordReaderWriter
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
        
        public void Write(List<Transaction> transactions, string file)
        {
            XmlWriter xmlWriter = XmlWriter.Create(file);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Transaction>));
            serializer.Serialize(xmlWriter, transactions);
        }
    }
}