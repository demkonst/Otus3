namespace Otus3.Abstractions
{
    public interface ITransactionRepository
    {
        void AddTransaction(ITransaction transaction);
        ITransaction[] GetTransactions();
    }
}
