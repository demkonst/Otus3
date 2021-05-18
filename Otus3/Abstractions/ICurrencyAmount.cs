namespace Otus3.Abstractions
{
    public interface ICurrencyAmount
    {
        string CurrencyCode { get; }
        decimal Amount { get; }
    }
}
