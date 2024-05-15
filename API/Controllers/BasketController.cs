using BLL_DB.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("add")]
        public IActionResult AddProductToBasket(int productId, int userId)
        {
            _basketService.AddProductToBasket(productId, userId);
            return Ok();
        }

        [HttpPost("changeQuantity")]
        public IActionResult ChangeBasketPositionQuantity(int productId, int userId, int newQuantity)
        {
            _basketService.ChangeBasketPositionQuantity(productId, userId, newQuantity);
            return Ok();
        }

        [HttpDelete("remove")]
        public IActionResult RemoveProductFromBasket(int productId, int userId)
        {
            _basketService.RemoveProductFromBasket(productId, userId);
            return Ok();
        }
    }
}
