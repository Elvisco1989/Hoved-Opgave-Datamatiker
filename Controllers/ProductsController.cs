using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public ProductsController(IProductRepo repo)
        {
            _repo = repo;
        }

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
                   
                })
                .ToList();

            return Ok(products);
        }

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
              
            };

            return Ok(dto);
        }


        [HttpPost]
        public ActionResult<ProductDto> AddProduct([FromBody] ProductDto dto)
        {
            var product = new Product
            {
                //ProductId = dto.ProductId,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
              
            };

            _repo.Addproduct(product);

            dto.ProductId = product.Id; // ensure returned DTO has ID

            return CreatedAtAction(nameof(GetProductById), new { id = dto.ProductId }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto dto)
        {
            if (id != dto.ProductId)
            {
                return BadRequest();
            }

            var updated = new Product
            {
                Id = dto.ProductId,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
               
            };

            _repo.UpdateProduct(updated);
            return NoContent();
        }
    }
}
