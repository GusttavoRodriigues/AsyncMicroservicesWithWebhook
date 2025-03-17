using Microsoft.AspNetCore.Mvc;
using OrderApi.Repository;
using Shared.DTOs;
using Shared.Models;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;
        public OrderController(IOrder order)
        {
            _order = order;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddOrder(Order order)
        {
            var response = await _order.AddOrderAsync(order);
            return response.Flag ? Ok(response) : BadRequest();
        }

    }
}
