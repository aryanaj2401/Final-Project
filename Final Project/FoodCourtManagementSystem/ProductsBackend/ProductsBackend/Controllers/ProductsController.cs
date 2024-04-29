using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsBackend.DTO;
using ProductsBackend.Handlers;
using ProductsBackend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;

        }
        // GET: api/<ProductsController>

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductInputDTO productInput)
        {
            try
            {
                var x = await _mediator.Send(new ProductInputCommand(productInput));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      //  [Authorize (Roles= "user,admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {

            try
            {
                var x = await _mediator.Send(new GetAllProductsQuery());
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/GetProductsDetailsWithCategories", Name = "GetProductsDetailsWithCategories")]
        public async Task < IActionResult> GetProductsDetailsWithCategories()
        {
            try
            {
                var x = await _mediator.Send(new GetProductsDetailsWithCategoriesQuery());
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/GetProductWithTime", Name = "GetProductWithTime")]
        public async Task<IActionResult> GetProductWithTime()
        {
            try
            {
                var x = await _mediator.Send(new GetProductWithTimeQuery());
                return Ok(x);
                
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("/EditAProduct", Name = "EditAProduct")]
        public async Task<IActionResult> EditAProduct( Product product)
        {
            try
            {
                var x = await _mediator.Send(new PutAProductCommand(product));
                return Ok(x);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}")]

        public async Task <IActionResult> GetProductsByCategoryId(int id)
        {
            try
            {
                var x = await _mediator.Send(new GetProductsByCategoryQuery(id));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        [HttpPut()]
        public async Task<IActionResult> ApproveAProduct(ApproveProductDTO approve)
        {
            try
            {
                await _mediator.Send(new ApproveAProductCommand(approve.ProductId, approve.QuantityApproced));
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAProduct(int id)
        {
            try
            {
                var x = await _mediator.Send(new DeleteAProductQuery(id));
                return Ok(x);

            }
            catch(Exception ex) 
            {
            return BadRequest(ex.Message);
            }
        }

        // PUT api/<ProductsController>/5

    }
}
