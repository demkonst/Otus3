using System.Collections.Generic;
using Otus3.Abstractions;

namespace Otus3
{
    public class InMemoryTransactionRepository: ITransactionRepository
    {
        private readonly List<ITransaction> _transactions = new();

        public void AddTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public ITransaction[] GetTransactions()
        {
            return _transactions.ToArray();
        }
    }
}
