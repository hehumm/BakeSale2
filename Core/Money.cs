namespace BakeSale2.Core
{
    public class Money
    {
        public decimal[] Denominations { get; }

        public Money(decimal[] denominations)
        {
            Denominations = denominations;
        }
    }
}
