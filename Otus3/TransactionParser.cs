using System;
using Otus3.Abstractions;

namespace Otus3
{
    public class TransactionParser: ITransactionParser
    {
        public ITransaction Parse(string input)
        {
            var parts = input.Split(" ");

            if (parts.Length != 6)
            {
                throw new ArgumentException(nameof(parts));
            }

            var amount = new CurrencyAmount
            {
                Amount = int.Parse(parts[1]),
                CurrencyCode = parts[2]
            };

            var date = DateTime.Parse(parts[5]);

            ITransaction result = parts[0] switch
            {
                "Трата" => new Expense
                {
                    Amount = amount,
                    Category = parts[3],
                    Date = date,
                    Destination = parts[4]
                },
                "Доход" => new Income
                {
                    Amount = amount,
                    Category = parts[3],
                    Date = date,
                    Destination = parts[4]
                },
                "Перевод" => new Transfer
                {
                    Amount = amount,
                    Category = parts[3],
                    Date = date,
                    Destination = parts[4]
                },

                _ => throw new ArgumentOutOfRangeException(nameof(parts))
            };

            return result;
        }
    }
}
