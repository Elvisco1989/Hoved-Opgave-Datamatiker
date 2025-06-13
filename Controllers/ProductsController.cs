using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af produktdata såsom oprettelse, hentning og opdatering af produkter.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repo;
        private readonly IProductService _productService;

        /// <summary>
        /// Initialiserer en ny instans af <see cref="ProductsController"/>.
        /// </summary>
        /// <param name="repo">Produktrepository til databaseadgang.</param>
        public ProductsController(IProductRepo repo, IProductService productService)
        {
            _repo = repo;
            _productService = productService;
        }

        /// <summary>
        /// Henter alle produkter.
        /// </summary>
        /// <returns>En liste af <see cref="ProductDto"/> objekter.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = _repo.Getproducts()
                .Select(p => new ProductDto
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Stock = p.Stock,
                    ImagePath = p.ImagePath,
                })
                .ToList();

            return Ok(products);
        }

        /// <summary>
        /// Henter et enkelt produkt baseret på ID.
        /// </summary>
        /// <param name="id">ID på det ønskede produkt.</param>
        /// <returns>Et <see cref="ProductDto"/> objekt hvis fundet, ellers 404 Not Found.</returns>
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var p = _repo.Getproduct(id);
            if (p == null) return NotFound();

            var dto = new ProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                ImagePath = p.ImagePath,
            };

            return Ok(dto);
        }

        /// <summary>
        /// Tilføjer et nyt produkt til databasen.
        /// </summary>
        /// <param name="dto">Produktdata som <see cref="ProductDto"/>.</param>
        /// <returns>Det oprettede produkt med genereret ID.</returns>
        [HttpPost]
        public ActionResult<ProductDto> AddProduct([FromBody] ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
                ImagePath = dto.ImagePath,
            };

            _repo.Addproduct(product);

            dto.ProductId = product.Id; // Sæt ID for returnering

            return CreatedAtAction(nameof(GetProductById), new { id = dto.ProductId }, dto);
        }

        /// <summary>
        /// Opdaterer et eksisterende produkt baseret på ID.
        /// </summary>
        /// <param name="id">Produktets ID som skal opdateres.</param>
        /// <param name="dto">Opdaterede produktdata.</param>
        /// <returns>NoContent hvis opdatering er succesfuld, ellers BadRequest.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto dto)
        {
            if (id != dto.ProductId)
            {
                return BadRequest("ID mismatch mellem route og body.");
            }

            var updated = new Product
            {
                Id = dto.ProductId,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
                ImagePath = dto.ImagePath,
            };

            _repo.UpdateProduct(updated);
            return NoContent();
        }

        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repo.Getproduct(id); // Get product by ID
            if (product == null)
                return NotFound(); // 404 if product not found

            _repo.Delete(product); // Call delete method
            return NoContent(); // 204 success, no body
        }


        [HttpGet("low-stock")]
        public IActionResult GetLowStockProducts()
        {
            var lowStock = _productService.GetLowStockProducts();
            return Ok(lowStock);
        }

    }
}
