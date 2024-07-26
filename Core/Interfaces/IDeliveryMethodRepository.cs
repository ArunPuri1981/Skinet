using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IDeliveryMethodRepository
    {
        Task<DeliveryMethod>GetDeliveryMethodByIdAsync(int id);
    }
}