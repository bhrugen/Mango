using AutoMapper;
using Mango.CouponAPI.Data;
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
    public async Task<ActionResult<ResponseDto>> GetCoupon()
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
    [HttpGet("{couponid}")]
    public async Task<ActionResult<Coupon>> GetCoupon(int couponid)
    {
        try
        {

        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = ex.Message;
        }
        if (!_response.IsSuccess) return BadRequest(_response);
        return Ok(_response);




        var coupon = await _context.Coupons.FindAsync(couponid);

        if (coupon == null)
        {
            return NotFound();
        }

        return coupon;
    }

    // PUT: api/Coupon/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{couponid}")]
    public async Task<IActionResult> PutCoupon(int? couponid, Coupon coupon)
    {
        if (couponid != coupon.CouponId)
        {
            return BadRequest();
        }

        _context.Entry(coupon).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CouponExists(couponid))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Coupon
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Coupon>> PostCoupon(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCoupon", new { couponid = coupon.CouponId }, coupon);
    }

    // DELETE: api/Coupon/5
    [HttpDelete("{couponid}")]
    public async Task<IActionResult> DeleteCoupon(int? couponid)
    {
        var coupon = await _context.Coupons.FindAsync(couponid);
        if (coupon == null)
        {
            return NotFound();
        }

        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CouponExists(int? couponid)
    {
        return _context.Coupons.Any(e => e.CouponId == couponid);
    }
}
