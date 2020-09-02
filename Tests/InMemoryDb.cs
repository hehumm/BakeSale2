using System.Collections.Generic;
using BakeSale2.Core;
using BakeSale2.Services;

namespace BakeSale2.Tests
{
    public class InMemoryDb
    {
        public Vendor Vendor { get; set; }

        private readonly Money _money = new Money(new []{100,50,20,10,5,1,0.5M,0.25M,0.1M,0.05M,0.01M});
        private readonly Dictionary<string,ProductWithWarehouseStockCount> _stock = new Dictionary<string, ProductWithWarehouseStockCount>()
        {
            {"Brownie", new ProductWithWarehouseStockCount() {Product = new Product() {Name = "Brownie", Price = 0.65M}, Count = 48}},
            {"Muffin", new ProductWithWarehouseStockCount() {Product = new Product() {Name = "Muffin", Price = 1}, Count = 36}},
            {"Cake Pop", new ProductWithWarehouseStockCount() {Product = new Product() {Name = "Cake Pop", Price = 1.35M}, Count = 24}},
            {"Water", new ProductWithWarehouseStockCount() {Product = new Product() {Name = "Water", Price = 1.5M}, Count = 30}}
        };

        public InMemoryDb()
        {
            Vendor = new Vendor(_money, _stock);
        }

    }
}