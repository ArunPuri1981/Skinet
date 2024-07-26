using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DeliveryMethodRepository : IDeliveryMethodRepository
    {
        private readonly StoreContext _storeContext;
        public DeliveryMethodRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id)
        {
            return await _storeContext.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}