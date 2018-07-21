using System;

namespace Flogging.Console.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal TotalReturns { get; set; }
    }
}
