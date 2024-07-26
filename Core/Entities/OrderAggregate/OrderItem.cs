using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered productItemOrdered, decimal price, int quantity)
        {
            ItemOrdered = productItemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}