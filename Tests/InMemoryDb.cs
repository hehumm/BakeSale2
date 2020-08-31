using BakeSale2.Core;
using BakeSale2.Domain;

namespace BakeSale2.Tests
{
    public class InMemoryDb
    {
        public Vendor Vendor { get; set; } = new Vendor();

        private readonly ProductWithCount _brownie = new ProductWithCount() {Product = new Product() {Name = "Brownie", Price = 0.65M}, Count = 48};
        private readonly ProductWithCount _muffin = new ProductWithCount() {Product = new Product() {Name = "Muffin", Price = 1}, Count = 48};
        private readonly ProductWithCount _cakePop = new ProductWithCount() {Product = new Product() {Name = "Cake Pop", Price = 1.35M}, Count = 48};
        private readonly ProductWithCount _water = new ProductWithCount() {Product = new Product() {Name = "Water", Price = 1.5M}, Count = 48};

        public InMemoryDb()
        {
            Vendor.Stock.Add("Brownie", _brownie);
            Vendor.Stock.Add("Muffin", _muffin);
            Vendor.Stock.Add("Cake Pop", _cakePop);
            Vendor.Stock.Add("Water", _water);
        }

    }
}