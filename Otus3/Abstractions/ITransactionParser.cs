namespace Otus3.Abstractions
{
    public interface ITransactionParser
    {
        ITransaction Parse(string input);
    }
}
