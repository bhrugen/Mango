using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mango.CouponAPI.Models;
using Mango.CouponAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class CouponAPIController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CouponAPIController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Coupon
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Coupon>>> GetCoupon()
    {
        return await _context.Coupons.ToListAsync();
    }

    // GET: api/Coupon/5
    [HttpGet("{couponid}")]
    public async Task<ActionResult<Coupon>> GetCoupon(int couponid)
    {
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
