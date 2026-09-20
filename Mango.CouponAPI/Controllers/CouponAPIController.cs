using AutoMapper;
using Mango.CouponAPI.Data;
using Mango.CouponAPI.Migrations;
using Mango.CouponAPI.Models;
using Mango.CouponAPI.Models.Dto;
using Mango.Serives.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CouponAPIController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private ResponseDto _response;
    private IMapper _mapper;
    public CouponAPIController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _response = new ResponseDto();
    }

    // GET: api/Coupon
    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetAllCoupons()
    {
        try
        {
            IEnumerable<Coupon> objList = await _context.Coupons.ToListAsync();
            _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
        }
        catch(Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage =ex.Message;
        }
        if(!_response.IsSuccess) return BadRequest(_response);
        return Ok(_response);
    }

    // GET: api/Coupon/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Coupon>> GetCouponById(int id)
    {
        try
        {
            var obj = await _context.Coupons.FindAsync(id);
            _response.Result = _mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = ex.Message;
        }
        if (!_response.IsSuccess) return BadRequest(_response);
        return Ok(_response);
       
    }

    // PUT: api/Coupon/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> UpdateCoupon([FromBody]CouponDto couponDto)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(couponDto);
            if (obj.CouponId == 0)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "CouponId is required for update.";
                return BadRequest(_response);
            }
            _context.Coupons.Update(obj);
            await _context.SaveChangesAsync();
            _response.Result = _mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = ex.Message;
        }
        if (!_response.IsSuccess) return BadRequest(_response);
        return Ok(_response);
     
    }

    // POST: api/Coupon
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Coupon>> CreateCoupon([FromBody]CouponDto couponDto)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(couponDto);
            _context.Coupons.Add(obj);
            await _context.SaveChangesAsync();
            _response.Result = _mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = ex.Message;
        }
        if (!_response.IsSuccess) return BadRequest(_response);
        return Ok(_response);
    }

    // DELETE: api/Coupon/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCoupon(int id)
    {
        try
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if(coupon == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Coupon not found.";
                return NotFound(_response);
            }
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
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
