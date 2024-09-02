using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        //private readonly Order _order;
        private readonly IDeliveryMethodRepository _deliveryMethod;
        private readonly IProductRepository _product;
        private readonly IBasketRepository _basketRepository;
        private readonly StoreContext _storeContext;
        public OrderService(StoreContext storeContext, IDeliveryMethodRepository deliveryMethod, IProductRepository product, IBasketRepository basketRepository)//Order order, 
        {
            _storeContext = storeContext;
            _basketRepository = basketRepository;
            _deliveryMethod = deliveryMethod;
            _product = product;
            //_order = order;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get basket from repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
            {
                return null; // Handle case when the basket is not found
            }

            //get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _product.GetProductByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            //get the delivery method
            var deliveryMethod = await _deliveryMethod.GetDeliveryMethodByIdAsync(deliveryMethodId);

            //calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

            //TODO: save to db
            await _storeContext.Orders.AddAsync(order);
            var result = await _storeContext.SaveChangesAsync();

            if (result<=0) return null;

            //Delete Basket if order is add
            await _basketRepository.DeleteBasketAsync(basketId);    
            //return order

            return order;
        }
        
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _storeContext.DeliveryMethods.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
             return await _storeContext.Orders
             .Include(o => o.OrderItems)
             .Where(o=>o.Id==id && o.BuyerEmail==buyerEmail)
             .FirstOrDefaultAsync();
                
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _storeContext.Orders
                    .Include(o=>o.OrderItems)
                    .Include(o=>o.DeliveryMethod)
                    .Where(o=>o.BuyerEmail==buyerEmail)
                    .ToListAsync();
        }
    }
}