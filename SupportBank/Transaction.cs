using System;

namespace SupportBank
{
    public class Transaction
    {
        public DateTime Date { get; set; } 
        public string From { get; set; }
        public string To { get; set; }
        public string Narrative { get; set; }
        public float Amount { get; set; }
            
        public Transaction(DateTime Date, string From, string To, string Narrative, float Amount)
        {
            this.Date = Date;
            this.From = From;
            this.To = To;
            this.Narrative = Narrative;
            this.Amount = Amount;
        }
    }
}