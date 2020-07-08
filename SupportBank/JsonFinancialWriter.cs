using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SupportBank
{
    public class JsonFinancialWriter : IFinancialRecordWriter
    {
        public void Write(List<Transaction> transactions, string file)
        {
            File.WriteAllText(file,
                JsonConvert.SerializeObject(transactions, Newtonsoft.Json.Formatting.Indented));
        }
    }
}