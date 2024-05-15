using Microsoft.AspNetCore.Mvc;
using BLL_DB.DTOModels;
using BLL_DB.ServiceInterfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<ProductResponseDTO>> GetAllProducts()
        {
            var products = _productService.GetProducts(null, null, null, null, true);
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<ProductResponseDTO> GetProduct(int id)
        {
            var product = _productService.GetProducts(null, null, null, id, true).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public ActionResult<ProductResponseDTO> AddProduct(ProductRequestDTO productDTO)
        {
            _productService.AddProduct(productDTO);
            return CreatedAtAction(nameof(GetProduct), new { id = productDTO.Name }, productDTO);
        }

        // PUT: api/Products/5/Activate
        [HttpPut("{id}/Activate")]
        public IActionResult ActivateProduct(int id)
        {
            _productService.ActivateProduct(id);
            return NoContent();
        }

        // PUT: api/Products/5/Deactivate
        [HttpPut("{id}/Deactivate")]
        public IActionResult DeactivateProduct(int id)
        {
            _productService.DeactivateProduct(id);
            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
