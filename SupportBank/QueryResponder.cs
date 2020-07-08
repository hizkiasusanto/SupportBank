using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportBank
{
    public class QueryResponder
    {
        public static void PrintQueryCommandResult(string inputCommand, List<Transaction> transactions)
        {
            if (inputCommand.Equals("List All"))
            {
                var accountsList = ListAll(transactions);
                accountsList.ForEach(delegate(Account account) { account.Print(); });
            }

            else if (inputCommand.StartsWith("List "))
            {
                string name = inputCommand.Substring(5);
                var account = ListOne(name, transactions);
                if (account.ListOfTransactions.Count != 0)
                {
                    account.Print();
                }
                else
                {
                    Console.WriteLine($"Account named {name} doesn't exist");
                }
            }

            else
            {
                Console.WriteLine("Invalid command");
            }
        }

        private static List<Account> ListAll(List<Transaction> transactionsList)
        {
            var uniqueNames = transactionsList
                .Select(t => t.From)
                .Concat(transactionsList
                    .Select(t => t.To))
                .Distinct();

            var accounts = uniqueNames.Select(t => new Account(t)).ToList();

            foreach (Account account in accounts)
            {
                account.GetAccountBalance(transactionsList);
            }

            return accounts;
        }

        private static Account ListOne(string accountHolder, List<Transaction> transactionsList)
        {
            Account accResult = new Account(accountHolder);

            accResult.FilterTransactions(transactionsList);

            return accResult;
        }
    }
}