using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af kunders kurv (basket).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IProductRepo _productRepo;

        /// <summary>
        /// Initialiserer en ny instans af <see cref="BasketController"/>.
        /// </summary>
        /// <param name="basketService">Service til håndtering af kurv.</param>
        /// <param name="productRepo">Repository til produktdata.</param>
        public BasketController(IBasketService basketService, IProductRepo productRepo)
        {
            _basketService = basketService;
            _productRepo = productRepo;
        }

        /// <summary>
        /// Tilføjer et produkt til en kundes kurv.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <param name="item">Kurv-elementet med produkt-id og antal.</param>
        /// <returns>Et HTTP-resultat, som angiver succes eller fejl.</returns>
        [HttpPost("{customerId}/add")]
        public IActionResult AddToBasket(int customerId, [FromBody] BasketItemDto item)
        {
            var product = _productRepo.Getproduct(item.ProductId);

            if (product == null) return NotFound("Product not found");

            _basketService.AddToBasket(customerId, product, item.Quantity);
            return Ok("Added to basket");
        }

        [HttpPost("{customerId}/update")]
        public IActionResult UpdateBasketItem(int customerId, [FromBody] BasketItemDto item)
        {
            var product = _productRepo.Getproduct(item.ProductId);

            if (product == null) return NotFound("Product not found");

            _basketService.UpdateItemQuantity(customerId, item.ProductId, item.Quantity);
            return Ok("Updated basket item");
        }


        [HttpPost("{customerId}/remove")]
        public IActionResult RemoveItem(int customerId, [FromBody] BasketItemDto item)
        {
            var basket = _basketService.GetBasket(customerId);
            var existingItem = basket.Find(i => i.ProductId == item.ProductId);
            if (existingItem == null)
                return NotFound("Item not found in basket.");

            try
            {
                _basketService.RemoveItem(customerId, item.ProductId);
                return Ok("Removed item from basket");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        /// <summary>
        /// Henter en kundes kurv.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <returns>En liste af <see cref="OrderItem"/> i kundens kurv.</returns>
        [HttpGet("{customerId}")]
        public ActionResult<List<OrderItem>> GetBasket(int customerId)
        {
            return Ok(_basketService.GetBasket(customerId));
        }
    }
}
