namespace Otus3.Abstractions
{
    public interface ICurrencyConverter
    {
        ICurrencyAmount ConvertCurrency(ICurrencyAmount amount, string currencyCode);
    }
}
