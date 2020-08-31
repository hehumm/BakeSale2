using System;
using System.Collections.Generic;
using System.IO;
using BakeSale2.Core;

namespace BakeSale2.Domain
{
    public class Vendor
    {
        public Dictionary<string,ProductWithCount> Stock { get; set; } = new Dictionary<string, ProductWithCount>();

        public readonly TextWriter ErrorWriter = Console.Error;

        public List<decimal> ProcessTransaction(decimal sum, decimal moneyGiven, Dictionary<string,int> chosenItems)
        {
            decimal changeMoney = calculateMoney(sum, moneyGiven);
            if (changeMoney < 0)
            {
                ErrorWriter.WriteLine("Not enough money");
                return null;
            }
            bool isSufficientStock = reduceStock(chosenItems);
            if (!isSufficientStock)
                return null;
            List<decimal> coinsAndNotesToReturn = returnMoney(changeMoney);
            return coinsAndNotesToReturn;
        }

        private decimal calculateMoney(decimal sum, decimal moneyGiven)
        {
            return moneyGiven - sum;
        }

        private bool reduceStock(Dictionary<string, int> chosenItems)
        {
            foreach (var chosenItem in chosenItems)
            {
                if (Stock[chosenItem.Key].Count >= chosenItem.Value)
                {
                    Stock[chosenItem.Key].Count -= chosenItem.Value;
                }
                else
                {
                    ErrorWriter.WriteLine($"Not enough stock of {chosenItem.Key}");
                    return false;
                }
            }

            return true;
        }

        private List<decimal> returnMoney(decimal changeDue)
        {
            List<decimal> changeMoney = new List<decimal>();
            int i = 0;
            while (changeDue>0)
            {
                if (Money.BankNotesAndCoins[i]>changeDue)
                {
                    i++;
                }
                else
                {
                    changeMoney.Add(Money.BankNotesAndCoins[i]);
                    changeDue -= Money.BankNotesAndCoins[i];
                }
            }

            return changeMoney;
        }
    }
}