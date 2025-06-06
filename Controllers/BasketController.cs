// Importerer nødvendige namespaces til brug i controlleren.
using Hoved_Opgave_Datamatiker.Models; // Indeholder modelklasser som OrderItem.
using Hoved_Opgave_Datamatiker.Models.Dto; // Indeholder data transfer objects (DTO'er) som BasketItemDto.
using Hoved_Opgave_Datamatiker.Repository; // Indeholder repository interfaces og implementeringer.
using Hoved_Opgave_Datamatiker.Services; // Indeholder services, f.eks. IBasketService.
using Microsoft.AspNetCore.Http; // Indeholder typer relateret til HTTP-kontext.
using Microsoft.AspNetCore.Mvc; // Indeholder ASP.NET Core MVC-funktionalitet.
using System.Collections.Generic; // Gør det muligt at bruge generiske lister (List<T>).

namespace Hoved_Opgave_Datamatiker.Controllers // Definerer controllerens namespace.
{
    /// <summary>
    /// Controller til håndtering af kunders kurv (basket).
    /// </summary>
    [Route("api/[controller]")] // Angiver route-skabelonen for controlleren (fx api/basket).
    [ApiController] // Angiver, at dette er en API-controller med automatiske valideringer osv.
    public class BasketController : ControllerBase // Arver fra ControllerBase, som er grundlaget for API-controllere.
    {
        private readonly IBasketService _basketService; // Service der håndterer kurv-logik.
        private readonly IProductRepo _productRepo; // Repository der leverer produktdata.

        /// <summary>
        /// Initialiserer en ny instans af <see cref="BasketController"/>.
        /// </summary>
        /// <param name="basketService">Service til håndtering af kurv.</param>
        /// <param name="productRepo">Repository til produktdata.</param>
        public BasketController(IBasketService basketService, IProductRepo productRepo)
        {
            _basketService = basketService; // Initialiserer _basketService med afhængigheden.
            _productRepo = productRepo; // Initialiserer _productRepo med afhængigheden.
        }

        /// <summary>
        /// Tilføjer et produkt til en kundes kurv.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <param name="item">Kurv-elementet med produkt-id og antal.</param>
        /// <returns>Et HTTP-resultat, som angiver succes eller fejl.</returns>
        [HttpPost("{customerId}/add")] // Definerer en POST-endpoint til at tilføje produkt.
        public IActionResult AddToBasket(int customerId, [FromBody] BasketItemDto item)
        {
            var product = _productRepo.Getproduct(item.ProductId); // Henter produktet fra repo.

            if (product == null) return NotFound("Product not found"); // Returnerer 404 hvis produkt ikke findes.

            _basketService.AddToBasket(customerId, product, item.Quantity); // Tilføjer produkt til kurven.
            return Ok("Added to basket"); // Returnerer 200 OK ved succes.
        }

        /// <summary>
        /// Opdaterer antallet af et produkt i kurven.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <param name="item">Kurv-element med opdateret antal.</param>
        /// <returns>Statuskode som angiver resultatet.</returns>
        [HttpPost("{customerId}/update")] // Definerer en POST-endpoint til at opdatere produktmængde.
        public IActionResult UpdateBasketItem(int customerId, [FromBody] BasketItemDto item)
        {
            var product = _productRepo.Getproduct(item.ProductId); // Henter produktet.

            if (product == null) return NotFound("Product not found"); // Returnerer 404 hvis produkt ikke findes.

            _basketService.UpdateItemQuantity(customerId, item.ProductId, item.Quantity); // Opdaterer antal i kurv.
            return Ok("Updated basket item"); // Returnerer succesbesked.
        }

        /// <summary>
        /// Fjerner et produkt fra kurven.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <param name="item">Kurv-element med produkt-id.</param>
        /// <returns>Statuskode som angiver resultatet.</returns>
        [HttpPost("{customerId}/remove")] // Definerer en POST-endpoint til at fjerne produkt.
        public IActionResult RemoveItem(int customerId, [FromBody] BasketItemDto item)
        {
            var basket = _basketService.GetBasket(customerId); // Henter hele kurven.
            var existingItem = basket.Find(i => i.ProductId == item.ProductId); // Finder ønsket item.

            if (existingItem == null)
                return NotFound("Item not found in basket."); // Returnerer 404 hvis item ikke findes.

            try
            {
                _basketService.RemoveItem(customerId, item.ProductId); // Fjerner item fra kurven.
                return Ok("Removed item from basket"); // Returnerer succesbesked.
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Returnerer 400 BadRequest hvis noget går galt.
            }
        }

        /// <summary>
        /// Henter en kundes kurv.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <returns>En liste af <see cref="OrderItem"/> i kundens kurv.</returns>
        [HttpGet("{customerId}")] // Definerer en GET-endpoint for at hente kundens kurv.
        public ActionResult<List<OrderItem>> GetBasket(int customerId)
        {
            return Ok(_basketService.GetBasket(customerId)); // Returnerer kurv-indholdet med 200 OK.
        }
    }
}
