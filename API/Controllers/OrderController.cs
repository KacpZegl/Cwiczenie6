using Microsoft.AspNetCore.Mvc;
using BLL_DB.ServiceInterfaces;
using BLL_DB.DTOModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/Order/GenerateFromBasket
        [HttpPost("GenerateFromBasket")]
        public IActionResult GenerateOrderFromBasket(int userId)
        {
            _orderService.GenerateOrderFromBasket(userId);
            return Ok();
        }

        // PUT: api/Order/PayOrder
        [HttpPut("PayOrder")]
        public IActionResult PayOrder(int orderId, decimal amountPaid)
        {
            _orderService.PayOrder(orderId, amountPaid);
            return NoContent();
        }

        // GET: api/Order/GetOrders
        [HttpGet("GetOrders")]
        public ActionResult<IEnumerable<OrderResponseDTO>> GetOrders(string sortBy, int? orderIdFilter, bool? paidStatusFilter)
        {
            var orders = _orderService.GetOrders(sortBy, orderIdFilter, paidStatusFilter);
            return Ok(orders);
        }

        // GET: api/Order/GetOrderPositions/{orderId}
        [HttpGet("GetOrderPositions/{orderId}")]
        public ActionResult<IEnumerable<OrderPositionResponseDTO>> GetOrderPositions(int orderId)
        {
            var orderPositions = _orderService.GetOrderPositions(orderId);
            return Ok(orderPositions);
        }
    }
}
