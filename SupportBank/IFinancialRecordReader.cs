using System.Collections.Generic;

namespace SupportBank
{
    public interface IFinancialRecordReader
    {
        List<Transaction> Read(string file);
    }
}