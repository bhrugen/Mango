using AutoMapper;
using Mango.MessageBus;
using Mango.OrderAPI.Data;
using Mango.OrderAPI.Models;
using Mango.OrderAPI.Models.Dto;
using Mango.OrderAPI.Service.IService;
using Mango.Serives.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.OrderAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderAPIController(ApplicationDbContext db, IMapper mapper, IProductService productService,
             IMessageBus messageBus, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
        }
        [HttpGet("GetOrders", Name = "GetOrders")]
        public async Task<ActionResult<ResponseDto>> GetOrders(string? userId="")
        {
            try
            {
                IEnumerable<OrderHeader> objList;

                if(string.IsNullOrEmpty(userId))
                {
                    objList = await _db.OrderHeader.AsNoTracking().ToListAsync();
                }
                else
                {
                    objList = await _db.OrderHeader.AsNoTracking().Where(u => u.UserId == userId).ToListAsync();
                }
                    _response.Result = _mapper.Map<List<OrderHeaderDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }


        [HttpGet("GetOrder/{id:int}", Name = "GetOrderById")]
        public async Task<ActionResult<ResponseDto>> GetOrderById(int id)
        {
            try
            {
                OrderHeader? orderHeader = 
                    await _db.OrderHeader.AsNoTracking().Where(u => u.OrderHeaderId == id).FirstOrDefaultAsync();

                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
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
