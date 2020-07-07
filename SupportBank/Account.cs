namespace SupportBank
{
    public class Account
    {
        public string AccountHolder;
        public float AccountBalance;

        public Account(string name, float balance)
        {
            this.AccountHolder = name;
            this.AccountBalance = balance;
        }

        public void ChangeBalance(float transaction)
        {
            this.AccountBalance += transaction;
        }
    }
}