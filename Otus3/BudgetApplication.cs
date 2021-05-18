using System;
using System.Linq;
using Otus3.Abstractions;

namespace Otus3
{
    public class BudgetApplication: IBudgetApplication
    {
        private readonly ITransactionRepository _repository;
        private readonly ITransactionParser _parser;
        private readonly ICurrencyConverter _converter;

        public BudgetApplication(ITransactionRepository repository,
            ITransactionParser parser, ICurrencyConverter converter)
        {
            _repository = repository;
            _parser = parser;
            _converter = converter;
        }

        public void AddTransation(string input)
        {
            var transaction = _parser.Parse(input);
            _repository.AddTransaction(transaction);
        }

        public void OutputTransactions()
        {
            var transactions = _repository.GetTransactions();
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"{transaction.Date:yyyy-MM-dd}: {transaction.Amount.Amount:F} {transaction.Amount.CurrencyCode} {transaction.Category} {transaction.Destination}");
            }
        }

        public void OutputBalanceInCurrency(string currencyCode)
        {
            var transactions = _repository.GetTransactions();

            var balance = transactions
                .Select(x => _converter.ConvertCurrency(x.Amount, currencyCode))
                .Sum(x => x.Amount);

            Console.WriteLine($"{balance:F}");
        }
    }
}
