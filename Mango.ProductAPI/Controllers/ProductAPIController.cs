using AutoMapper;
using Mango.ProductAPI.Data;
using Mango.ProductAPI.Models;
using Mango.ProductAPI.Models.Dto;
using Mango.Serives.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/products")]
[ApiController]
public class ProductAPIController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private ResponseDto _response;
    private IMapper _mapper;

    public ProductAPIController(ApplicationDbContext db, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _response = new ResponseDto();
    }

    // GET: api/products
    [HttpGet]
    [AllowAnonymous]
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

    // GET: api/products/5
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseDto>> GetProductById(int id)
    {
        try
        {
            var obj = await _db.Products.FindAsync(id);
            if (obj == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Product not found.";
                return NotFound(_response);
            }
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

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateProduct([FromForm] ProductDto productDto)
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

            if (productDto.Image != null)
            {
                await SaveProductImageAsync(obj, productDto.Image);
                await _db.SaveChangesAsync();
            }

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

    // PUT: api/products
    [HttpPut]
    public async Task<ActionResult<ResponseDto>> UpdateProduct([FromForm] ProductDto productDto)
    {
        try
        {
            var obj = await _db.Products.FindAsync(productDto.ProductId);
            if (obj == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Product not found.";
                return NotFound(_response);
            }

            obj.Name = productDto.Name;
            obj.Price = productDto.Price;
            obj.Description = productDto.Description;
            obj.CategoryName = productDto.CategoryName;

            if (productDto.Image != null)
            {
                DeleteProductImage(obj);
                await SaveProductImageAsync(obj, productDto.Image);
            }

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

    // DELETE: api/products/5
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

            DeleteProductImage(product);

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

    private async Task SaveProductImageAsync(Product product, IFormFile image)
    {
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string productPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages");
        Directory.CreateDirectory(productPath);

        var filePath = Path.Combine(productPath, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        product.ImageUrl = $"{baseUrl}/ProductImages/{fileName}";
        product.ImageLocalPath = filePath;
    }

    private void DeleteProductImage(Product product)
    {
        if (!string.IsNullOrEmpty(product.ImageLocalPath) && System.IO.File.Exists(product.ImageLocalPath))
        {
            System.IO.File.Delete(product.ImageLocalPath);
        }
        product.ImageUrl = null;
        product.ImageLocalPath = null;
    }
}
