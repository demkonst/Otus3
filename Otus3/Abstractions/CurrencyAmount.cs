using System;

namespace Otus3.Abstractions
{
    public class CurrencyAmount: ICurrencyAmount
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }

        protected bool Equals(CurrencyAmount other)
        {
            return CurrencyCode == other.CurrencyCode && Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CurrencyAmount) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrencyCode, Amount);
        }
    }
}
