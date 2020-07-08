using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SupportBank
{
    public class Transaction
    {
        [JsonProperty("Date")] public DateTime Date { get; set; }
        [JsonProperty("FromAccount")] public string From { get; set; }
        [JsonProperty("ToAccount")] public string To { get; set; }
        [JsonProperty("Narrative")] public string Narrative { get; set; }
        [JsonProperty("Amount")] public float Amount { get; set; }
        
        private Transaction() {}

        public Transaction(DateTime Date, string From, string To, string Narrative, float Amount)
        {
            this.Date = Date;
            this.From = From;
            this.To = To;
            this.Narrative = Narrative;
            this.Amount = Amount;
        }

        public void Print()
        {
            Console.WriteLine($"Date: {this.Date}");
            Console.WriteLine($"To: {this.To}");
            Console.WriteLine($"From: {this.From}");
            Console.WriteLine($"Narrative: {this.Narrative}");
            Console.WriteLine($"Amount: {this.Amount}");
            Console.WriteLine();
        }
    }
}