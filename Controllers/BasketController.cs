using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IProductRepo _productRepo;

        public BasketController(IBasketService basketService, IProductRepo productRepo)
        {
            _basketService = basketService;
            _productRepo = productRepo;
        }

        [HttpPost("{customerId}/add")]
        public IActionResult AddToBasket(int customerId, [FromBody] BasketItemDto item)
        {
            var product = _productRepo.Getproduct(item.ProductId);
            if (product == null) return NotFound("Product not found");

            _basketService.AddToBasket(customerId, product, item.Quantity);
            return Ok("Added to basket");
        }


        [HttpGet("{customerId}")]
        public ActionResult<List<OrderItem>> GetBasket(int customerId)
        {
            return Ok(_basketService.GetBasket(customerId));
        }
    }

}
