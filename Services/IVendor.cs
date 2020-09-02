using System.Collections.Generic;
using BakeSale2.Core;

namespace BakeSale2.Services
{
    public interface IVendor
    {
        Dictionary<string, ProductWithWarehouseStockCount> Stock { get; }
        List<decimal> ProcessTransaction(decimal sum, decimal moneyGiven, Dictionary<string,int> chosenItems);
    }
}