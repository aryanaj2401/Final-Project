using MediatR;
using Microsoft.AspNetCore.Mvc;
using BackendUsers.Handlers.OrderHandler;
using BackendUsers.DTO.OrderDTOs;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    { private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "admin,user")]
        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult>  GetAllOrders ()
        {
           try
            {
                var x = await _mediator.Send(new GetAllOrdersQuery(Request.Headers.Authorization));
                return Ok(x);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
       [Authorize(Roles = "admin,user")]
        // GET api/<OrdersController>/5
        [HttpGet("{email}")]
        public async Task<IActionResult> GetAllItemsInCart(string email)
        {
            try
            {
                var x = await _mediator.Send(new GetCartOrdersByEmailIdQuery(email,Request.Headers.Authorization));
                return Ok(x);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        // POST api/<OrdersController>
        [HttpPost("/AddToCart", Name = "AddToCart")]
        public async Task <IActionResult> AddToCart(OrderInputDTO orderdto)
        { 
            try
            {
                
                var x = await _mediator.Send(new AddToCartCommand(orderdto,Request.Headers.Authorization));
                return Ok(x);
                    }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "user")]
        [HttpPost("/CreateAnOrder", Name = "CreateAnOrder")]

        public async Task <IActionResult> CreateAnOrder(OrderInputDTO orderdto)
        {

            var i = Request.Headers.Authorization;
            try
            {
                var x = await _mediator.Send(new CreateAnOrderCommand(orderdto));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task <IActionResult> PutNewStatus(PutDTO putDTO)
        {
            try
            {
                var x = await _mediator.Send(new PutNewStatusCommand(putDTO.NewStatus, putDTO.OrderId));
                return Ok(x);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
