using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {

        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Recived")]
        PaymentRecived,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,

    }
}