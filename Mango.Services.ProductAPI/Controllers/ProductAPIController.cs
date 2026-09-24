using AutoMapper;
using Mango.ProductAPI.Data;
using Mango.ProductAPI.Models;
using Mango.ProductAPI.Models.Dtos;
using Mango.Serives.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public ProductAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllProducts()
        {
            try
            {
                IEnumerable<Product> objList = await _db.Products.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }

        // GET: api/Product/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var obj = await _db.Products.FindAsync(id);
                _response.Result = _mapper.Map<ProductDto>(obj);
                if (obj == null)
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);

        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto.ProductId > 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "ProductId must be empty for creating a product.";
                    return BadRequest(_response);
                }
                Product obj = _mapper.Map<Product>(productDto);

                _db.Products.Add(obj);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDto);
                if (obj.ProductId == 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "ProductId is required for update.";
                    return BadRequest(_response);
                }
                _db.Products.Update(obj);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);

        }

        // DELETE: api/Product/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Product not found.";
                    return NotFound(_response);
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);

        }
    }
}
