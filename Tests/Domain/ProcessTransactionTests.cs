using System.Collections.Generic;
using BakeSale2.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BakeSale2.Tests.Domain
{
    [TestClass]
    public class ProcessTransactionTests
    {
        private Vendor _vendor;
        private decimal _sum;
        private decimal _moneyGiven;
        private Dictionary<string, int> _chosenItems;

        [TestInitialize]
        public void Setup()
        {
            var db = new InMemoryDb();
            _vendor = db.Vendor;
        }

        private void configureDefaultParams()
        {
            _sum = 3.35M;
            _moneyGiven = 5;
            _chosenItems = new Dictionary<string, int>()
            {
                {"Brownie", 1},
                {"Cake Pop", 2}
            };
        }

        [TestMethod]
        public void Success_ReturnsCorrectChangeAmount()
        {
            configureDefaultParams();
            decimal expectedChange = _moneyGiven - _sum;
            List<decimal> returnedCoins = _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            decimal sumOfReturnedCoins = 0;
            foreach (var returnedCoin in returnedCoins)
            {
                sumOfReturnedCoins += returnedCoin;
            }
            Assert.IsNotNull(expectedChange);
            Assert.AreEqual(expectedChange, sumOfReturnedCoins);
        }

        [TestMethod]
        public void Success_DecreasedStock()
        {
            configureDefaultParams();
            foreach (var chosenItem in _chosenItems)
            {
                int expectedRemainingAmount = _vendor.Stock[chosenItem.Key].Count - chosenItem.Value;
                _vendor.ProcessTransaction(_sum, _moneyGiven, new Dictionary<string, int>()
                {
                    {chosenItem.Key, chosenItem.Value}
                });
                Assert.AreEqual(expectedRemainingAmount, _vendor.Stock[chosenItem.Key].Count);
            }
        }

        [TestMethod]
        public void Success_ReturnedOptimalChange()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Fail_NotEnoughMoney()
        {
            configureDefaultParams();
            _moneyGiven = _sum - _sum/2;
            var expectedChange = _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            Assert.IsNull(expectedChange);
        }

        [TestMethod]
        public void Fail_NotEnoughStock()
        {
            Assert.Inconclusive();
        }
    }
}
