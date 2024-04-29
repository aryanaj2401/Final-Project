using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackendUsers.Handlers.OrderHandler
{
    public class CreateAnOrderCommand : IRequest<OrderOutputDTO>
    {
        public OrderInputDTO OrderInputDTO { get; set; }
        public CreateAnOrderCommand(OrderInputDTO orderInput)
        {
            OrderInputDTO = orderInput;
        }
    }
    public class CreateAnOrderHandler : IRequestHandler<CreateAnOrderCommand, OrderOutputDTO>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public CreateAnOrderHandler(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderOutputDTO> Handle(CreateAnOrderCommand command, CancellationToken cancellationToken)
        {
            using (var httpclient = new HttpClient())
            {

                command.OrderInputDTO.email = command.OrderInputDTO.email.ToUpper();
                command.OrderInputDTO.Status_Of_Request = command.OrderInputDTO.Status_Of_Request.ToUpper();
                var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                var responsestring = await response.Content.ReadAsStringAsync();
                var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                    new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var productarr = productlist.Select(e => e.Id).ToList();


                foreach (var productid in command.OrderInputDTO.ProductIds)
                {
                    if (!productarr.Contains(productid.productId))
                        throw new Exception($"Product Id {productid} Does Not Exist");
                }
            

                var disabledproducts = productlist.Where(x => x.Deleted == true).Select(e => e.Id).ToList();

                foreach (var productid in command.OrderInputDTO.ProductIds)
                {

                    if (disabledproducts.Contains(productid.productId))
                        throw new Exception($"Product Id {productid.productId} is Deleted");
                       
                        
                }
                var check = await _db.Orders.Where(x => x.email == command.OrderInputDTO.email && x.Status_Of_Request == "CART").FirstOrDefaultAsync();
                if(check == null)
                {
                    throw new Exception("Nothing found In cart");

                }
                 
                check.Status_Of_Request = "ORDER";

                await _db.SaveChangesAsync();

                var order = new Order
                {
                    email = command.OrderInputDTO.email,
                    Status_Of_Request = command.OrderInputDTO.Status_Of_Request,
                    CreatedTime = DateTime.Now,
                    AcceptedTime = DateTime.Now
                };
                //_db.Orders.Add(order);
                //await _db.SaveChangesAsync();
                //foreach (var productId in command.OrderInputDTO.ProductIds)
                //{
                //    var orderProduct = new OrderProduct
                //    {
                //        OrderId = order.Id,

                //        ProductId = productId.productId,
                //        Quantity = productId.quantity
                //    };

                //    _db.OrderProducts.Add(orderProduct);

                //}

                //await _db.SaveChangesAsync();
                var output = new OrderOutputDTO()
                {
                    Id = order.Id,
                    email = order.email,
                    ProductIdandQuantity = command.OrderInputDTO.ProductIds,
                    Status_Of_Request = order.Status_Of_Request,
                    CreatedTime = DateTime.Now,
                    AcceptedTime = DateTime.Now


                };
                return output;



            }
        }
    }
}
