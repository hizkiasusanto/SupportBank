using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace SupportBank
{
    public class CsvFinancialWriter : IFinancialRecordWriter
    {
        public void Write(List<Transaction> transactions, string file)
        {
            var csvWriter = new StreamWriter(file);
            new CsvWriter(csvWriter, new CultureInfo("en-GB")).WriteRecords(transactions);
        }
    }
}