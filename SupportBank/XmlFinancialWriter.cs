using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SupportBank
{
    public class XmlFinancialWriter : IFinancialRecordWriter
    {
        public void Write(List<Transaction> transactions, string file)
        {
            XmlWriter xmlWriter = XmlWriter.Create(file);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Transaction>));
            serializer.Serialize(xmlWriter, transactions);
        }
    }
}