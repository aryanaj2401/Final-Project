using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsBackend.DTO;
using ProductsBackend.Handlers;
using ProductsBackend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
            
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var x = await _mediator.Send(new GetAllCategoriesQuery());
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoriesInputDTO categoriesInputDTO)
        {
            try
            {
                var x = await _mediator.Send(new CategoryInputCommand(categoriesInputDTO));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public async Task<IActionResult> Put(Category category)
        {
            try
            {
                var x = await _mediator.Send(new EditACategoryQuery(category));
                return Ok(x);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var x = await _mediator.Send(new DeleteACategoryCommand(id));
                return Ok(x);   

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
