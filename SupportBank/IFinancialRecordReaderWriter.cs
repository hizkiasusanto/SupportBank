using System.Collections.Generic;

namespace SupportBank
{
    public interface IFinancialRecordReaderWriter
    {
        List<Transaction> Read(string file);
        void Write(List<Transaction> transactions,string file);
    }
}