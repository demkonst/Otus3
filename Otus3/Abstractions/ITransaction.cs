using System;

namespace Otus3.Abstractions
{
    public interface ITransaction
    {
        ICurrencyAmount Amount { get; }
        string Category { get; }
        DateTimeOffset Date { get; }
        string Destination { get; }
    }

    public class Expense: ITransaction
    {
        public ICurrencyAmount Amount { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Destination { get; set; }
    }

    public class Transfer: ITransaction
    {
        public ICurrencyAmount Amount { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Destination { get; set; }
    }

    public class Income: ITransaction
    {
        public ICurrencyAmount Amount { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Destination { get; set; }
    }
}
