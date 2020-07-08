using System.Collections.Generic;

namespace SupportBank
{
    public interface IFinancialRecordWriter
    {
        void Write(List<Transaction> transactions,string file);
    }
}