namespace Otus3.Abstractions
{
    public interface IBudgetApplication
    {
        void AddTransation(string input);
        void OutputTransactions();
        void OutputBalanceInCurrency(string currencyCode);
    }
}
