using System;

namespace ModelsDTO
{
    public class Order
    {
        public Order(long consNumber, string product, decimal price, double amount, string unitType, double totalPrice)
        {
            ConsignmentNumber = consNumber;
            Product = product;
            Price = price;
            Amount = amount;
            UnitType = unitType;
            TotalPrice = totalPrice;
        }

        public Order(Order copiedOrder)
            : this(copiedOrder.ConsignmentNumber, copiedOrder.Product, copiedOrder.Price
                        , copiedOrder.Amount, copiedOrder.UnitType, copiedOrder.TotalPrice)
        {

        }

        public Order()
        {

        }

        public long ConsignmentNumber { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public string UnitType { get; set; }
        public double TotalPrice { get; set; }
    }
}
