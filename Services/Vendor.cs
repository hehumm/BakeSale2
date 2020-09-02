using System;
using System.Collections.Generic;
using System.IO;
using BakeSale2.Core;

namespace BakeSale2.Services
{
    public class Vendor : IVendor
    {
        public Dictionary<string,ProductWithWarehouseStockCount> Stock { get; }

        private readonly Money _money;
        public readonly TextWriter ErrorWriter = Console.Error;

        public Vendor(Money money, Dictionary<string,ProductWithWarehouseStockCount> stock)
        {
            _money = money;
            Stock = stock;
        }

        public List<decimal> ProcessTransaction(decimal sum, decimal moneyGiven, Dictionary<string,int> chosenItems)
        {
            List<decimal> denominationsToReturn = new List<decimal>();
            decimal changeMoney = calculateCustomerReturnMoney(sum, moneyGiven);
            if (changeMoney < 0)
            {
                ErrorWriter.WriteLine("Not enough money");
                return denominationsToReturn;
            }
            reduceStock(chosenItems);
            denominationsToReturn = findDenominationsToReturn(changeMoney);
            return denominationsToReturn;
        }

        private decimal calculateCustomerReturnMoney(decimal sum, decimal moneyGiven)
        {
            return moneyGiven - sum;
        }

        private void reduceStock(Dictionary<string, int> chosenItems)
        {
            foreach (var chosenItem in chosenItems)
            {
                if (Stock[chosenItem.Key].Count >= chosenItem.Value)
                {
                    Stock[chosenItem.Key].Count -= chosenItem.Value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{chosenItem.Key}: Not enough stock available.");
                }
            }
        }

        private List<decimal> findDenominationsToReturn(decimal changeDue)
        {
            List<decimal> changeMoney = new List<decimal>();
            int i = 0;
            while (changeDue>0)
            {
                if (_money.Denominations[i]>changeDue)
                {
                    i++;
                }
                else
                {
                    changeMoney.Add(_money.Denominations[i]);
                    changeDue -= _money.Denominations[i];
                }
            }

            return changeMoney;
        }
    }
}