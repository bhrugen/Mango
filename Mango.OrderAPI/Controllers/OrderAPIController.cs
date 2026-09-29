using AutoMapper;
using Mango.MessageBus;
using Mango.OrderAPI.Data;
using Mango.OrderAPI.Models;
using Mango.OrderAPI.Models.Dto;
using Mango.OrderAPI.Service.IService;
using Mango.OrderAPI.Utility;
using Mango.Serives.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Security.Claims;

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

        private bool IsAdmin =>
            User.Claims.Any(c => (c.Type == ClaimTypes.Role || c.Type == "role")
                                 && string.Equals(c.Value, SD.RoleAdmin, StringComparison.OrdinalIgnoreCase));

        private string? CurrentUserId =>
            User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [Authorize]
        [HttpGet("GetOrders", Name = "GetOrders")]
        public async Task<ActionResult<ResponseDto>> GetOrders(string? userId="")
        {
            try
            {
                // Non-admins can only ever see their own orders, whatever userId they pass.
                if (!IsAdmin)
                {
                    userId = CurrentUserId;
                    if (string.IsNullOrEmpty(userId)) return Forbid();
                }

                IQueryable<OrderHeader> query = _db.OrderHeader.AsNoTracking();
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(u => u.UserId == userId);
                }
                var objList = await query.OrderByDescending(u => u.OrderHeaderId).ToListAsync();
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


        [Authorize]
        [HttpGet("GetOrder/{id:int}", Name = "GetOrderById")]
        public async Task<ActionResult<ResponseDto>> GetOrderById(int id)
        {
            try
            {
                OrderHeader? orderHeader = await _db.OrderHeader.AsNoTracking()
                    .Include(u => u.OrderDetails)
                    .FirstOrDefaultAsync(u => u.OrderHeaderId == id);

                if (orderHeader == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = $"Order with ID {id} not found.";
                    return NotFound(_response);
                }
                if (!IsAdmin && orderHeader.UserId != CurrentUserId)
                {
                    return Forbid();
                }

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

        [HttpPost("CreateOrder", Name = "CreateOrder")]
        public async Task<ActionResult<ResponseDto>> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto=  _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.Email = cartDto.Email;
                orderHeaderDto.Name = cartDto.Name;
                orderHeaderDto.Phone = cartDto.Phone;
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
                orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal,2);
                OrderHeader orderCreated = _db.OrderHeader.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            if (!_response.IsSuccess) return BadRequest(_response);
            return Ok(_response);
        }


        [HttpPost("ConfirmOrder/{orderId:int}", Name = "ConfirmOrder")]
        public async Task<ActionResult<ResponseDto>> ConfirmOrder(int orderId)
        {
            try
            {
                OrderHeader orderHeader = await _db.OrderHeader.FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    orderHeader.Status = SD.Status_Approved;
                    await _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);


                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                    await _messageBus.PublishMessage(topicName, _response.Result);

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = $"Order with ID {orderId} not found.";
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


        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ActionResult<ResponseDto>> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            try
            {
                if (!IsAdmin) return Forbid();

                string[] validStatuses =
                {
                    SD.Status_Pending, SD.Status_Approved, SD.Status_ReadyForPickup,
                    SD.Status_Completed, SD.Status_Refunded, SD.Status_Cancelled
                };
                if (!validStatuses.Contains(status))
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = $"Invalid status '{status}'.";
                    return BadRequest(_response);
                }

                OrderHeader orderHeader = await _db.OrderHeader.FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    orderHeader.Status = status;
                    await _db.SaveChangesAsync();
                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = $"Order with ID {orderId} not found.";
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


    }
}
