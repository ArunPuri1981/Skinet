using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Error;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDto)
        {
            var email = HttpContext.User.RetrievedEmailFromPrincipal();
            var address = _mapper.Map<AddressDTO, Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId,
            orderDto.BasketId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "problem Creating order"));

            return Ok(order);

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderForUser()
        {
            var email = HttpContext.User.RetrievedEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrievedEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(order);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}