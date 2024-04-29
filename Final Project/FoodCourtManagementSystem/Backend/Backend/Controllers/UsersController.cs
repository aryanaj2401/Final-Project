using BackendUsers.Models;
using BackendUsers.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using BackendUsers.Handlers.UserHandlers;
using BackendUsers.DTO.LoginDTOs;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Handlers.OrderHandler;



namespace BackendUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // private readonly IMapper _mapper;
        
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            //  _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get_All_Users()
        {
            var x = await _mediator.Send(new All_Users_Query());
            return Ok(x);

        }

        [HttpPost]

        public async Task<IActionResult> Post_A_User([FromBody] UserInputDTO userdto)
        {
            try
            {

                var x = await _mediator.Send(new UserInputCommand(userdto));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

          

        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get_User_By_Id(string email)
        {
            try
            {
                var x = await _mediator.Send(new Get_User_By_id_Query(email));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("/Login", Name = "Login")]

        public async Task<IActionResult> Login_Check(LoginInputDTO lidto)
        {


            try
            {
                var username = lidto.Email;
                var password = lidto.Password;
                var x = await _mediator.Send(new LoginCommand(username, password));
                return Ok(x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/UpdatePassword", Name ="UpdatePassword")]

        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updto)
        {
            try 
            { updto.Email = updto.Email.ToUpper();

                var x = await _mediator.Send(new UpdatePasswordCommand(updto));
                return Ok(x);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/GetAllProducts", Name = "GetAllProducts")]
        public async Task <IActionResult> GetAllProducts()
        {
            try
            {
                
                using (var httpclient = new HttpClient()) {
                    
                    var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                    var responsestring  = await response.Content.ReadAsStringAsync();
                    var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                        new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true}) ;
                    return Ok(productlist);
                }
                HttpClient client = new();
               

                var x = await client.GetAsync("https://localhost:7199/api/Products");
             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("/GetAllOrders" , Name = "DisplayAllOrders" )]

        //public async Task <IActionResult> GetAllOrders()
        //{
        //    try
        //    {
              

        //    }
        //    catch (Exception ex) { }
        //}



      
       
    }
}
