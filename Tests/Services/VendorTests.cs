using System;
using System.Collections.Generic;
using BakeSale2.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BakeSale2.Tests.Services
{
    [TestClass]
    public class VendorTests
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
            configureDefaultParams();
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
        public void ProcessTransaction_Succeeds_GivenCorrectParameters()
        {
            decimal expectedChange = 1.65M;
            List<decimal> returnedDenominations = _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            decimal actualChange = 0;
            foreach (var returnedDenomination in returnedDenominations)
            {
                actualChange += returnedDenomination;
            }
            Assert.AreEqual(expectedChange, actualChange);
        }

        [TestMethod]
        public void ProcessTransaction_DecreasesStock_GivenCorrectParameters()
        {
            int expectedBrownieRemainingStock = 47;
            int expectedCakePopRemainingStock = 22;
            _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            Assert.AreEqual(expectedBrownieRemainingStock, _vendor.Stock["Brownie"].Count);
            Assert.AreEqual(expectedCakePopRemainingStock, _vendor.Stock["Cake Pop"].Count);
        }

        [TestMethod]
        public void ProcessTransaction_ReturnsCorrectDenominations_GivenCorrectParameters()
        {
            List<decimal> expectedReturnedDenominations = new List<decimal>(){1,0.5M,0.1M,0.05M}; //when change is $1.65
            List<decimal> actualReturnedDenominations = _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            Assert.AreEqual(4, actualReturnedDenominations.Count);
            for (int i = 0; i < expectedReturnedDenominations.Count; i++)
            {
                Assert.AreEqual(expectedReturnedDenominations[i],actualReturnedDenominations[i]);
            }
        }

        [TestMethod]
        public void ProcessTransaction_ReturnsEmptyCollection_NotEnoughMoney()
        {
            _moneyGiven = 1;
            var expectedChange = _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            Assert.AreEqual(0,expectedChange.Count);
        }

        [TestMethod]
        public void ProcessTransaction_ThrowsArgumentOutOfRangeException_NotEnoughStock()
        {
            _chosenItems = new Dictionary<string, int>()
            {
                {"Brownie", 999999}
            };
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                _vendor.ProcessTransaction(_sum, _moneyGiven, _chosenItems);
            });
        }
    }
}
