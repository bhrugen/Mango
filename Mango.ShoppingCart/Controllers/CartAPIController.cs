using AutoMapper;
using Mango.Serives.Shared.Models;
using Mango.ShoppingCartAPI.Data;
using Mango.ShoppingCartAPI.Migrations;
using Mango.ShoppingCartAPI.Models;
using Mango.ShoppingCartAPI.Models.Dto;
using Mango.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        private IProductService _productService;
        private ICouponService _couponService;

        public CartAPIController(ApplicationDbContext db, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _productService = productService;
            _couponService = couponService;
        }
        [HttpGet("GetCart/{userId}", Name = "GetCart")]
        public async Task<ActionResult<ResponseDto>> GetCart(string userId)
        {
            try
            {
                var cartHeaderFromdb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
                if (cartHeaderFromdb == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Cart header not found for the user.";
                    return NotFound(_response);
                }
                else
                {
                    CartDto cartDto = new CartDto
                    {
                        CartHeader = _mapper.Map<CartHeaderDto>(cartHeaderFromdb),
                        CartDetails = _mapper.Map<List<CartDetailsDto>>(await _db.CartDetails.AsNoTracking().Where(u => u.CartHeaderId == cartHeaderFromdb.CartHeaderId).ToListAsync())
                    };

                    IEnumerable<ProductDto> productDtos = await _productService.GetProducts();
                    foreach(var item in cartDto.CartDetails){
                        item.Product = productDtos.FirstOrDefault(p => p.ProductId == item.ProductId);
                        if (item.Product != null)
                        {
                            cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
                        }
                    }

                    //apply coupon
                    if(!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                    {
                        CouponDto coupon = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                        if(coupon != null && cartDto.CartHeader.CartTotal > coupon.MinAmount)
                        {
                            cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
                            cartDto.CartHeader.Discount = coupon.DiscountAmount;
                        }
                    }

                    _response.Result = cartDto;
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




        [HttpPost("ApplyCoupon", Name = "ApplyCoupon")]
        public async Task<ActionResult<ResponseDto>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeaderFromdb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromdb != null)
                {
                    cartHeaderFromdb.CouponCode = cartDto.CartHeader.CouponCode;
                    _db.CartHeaders.Update(cartHeaderFromdb);
                    await _db.SaveChangesAsync();

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Cart header not found for the user.";
                    return NotFound(_response);
                }
               
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }

        [HttpPost("RemoveCartDetails", Name = "RemoveCartDetails")]
        public async Task<ActionResult<ResponseDto>> RemoveCartDetails([FromBody] int cartDetailsId)
        {
            try
            {
               CartDetails cartDetails = await _db.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);
                if (cartDetails != null)
                {
                    _db.CartDetails.Remove(cartDetails);
                    await _db.SaveChangesAsync();

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Cart details not found for the user.";
                    return NotFound(_response);
                }

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }


        [HttpPost("CartUpsert", Name = "CartUpsert")]
        public async Task<ActionResult<ResponseDto>> CartUpsert([FromBody] CartDto cartDto)
        {
            try
            {
                if(cartDto!=null && cartDto.CartHeader!=null && cartDto.CartHeader.UserId!=null && cartDto.CartDetails!=null && cartDto.CartDetails.Count() > 0)
                {
                    var cartHeaderFromdb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                    var cartDetailsDto = cartDto.CartDetails.First();
                    if (cartHeaderFromdb == null)
                    {
                        CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                        CartDetails cartDetails= _mapper.Map<CartDetails>(cartDetailsDto);
                        cartDetails.CartHeader = cartHeader;
                        _db.CartDetails.Add(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        var cartDetailsFromDb = await _db.CartDetails.AsNoTracking()
                            .FirstOrDefaultAsync(u => u.ProductId == cartDetailsDto.ProductId && u.CartHeaderId == cartHeaderFromdb.CartHeaderId);

                        if (cartDetailsFromDb == null)
                        {
                            cartDetailsDto.CartHeaderId = cartHeaderFromdb.CartHeaderId;
                            _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDetailsDto));
                            await _db.SaveChangesAsync();
                        }
                        else
                        {
                            
                            cartDetailsDto.Count += cartDetailsFromDb.Count;
                            
                                cartDetailsDto.CartHeaderId = cartHeaderFromdb.CartHeaderId;
                            cartDetailsDto.CartDetailsId = cartDetailsFromDb.CartDetailsId;
                            if (cartDetailsDto.Count <= 0)
                            {
                                _db.CartDetails.Remove(_mapper.Map<CartDetails>(cartDetailsDto));
                            }
                            else
                            {
                                _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDetailsDto));
                            }
                            await _db.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Invalid cart data.";
                    return BadRequest(_response);
                }

                _response.Result = cartDto;
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
