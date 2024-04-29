using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackendUsers.Handlers.OrderHandler
{
    public class GetAllOrdersQuery : IRequest<List<OrderSummaryOutputDTO>>
    {
        public string _token { get; set; }

        public GetAllOrdersQuery(string token)
        {
            _token = token;
        }
    }
    public class GetAllOrdersHandlers : IRequestHandler<GetAllOrdersQuery, List<OrderSummaryOutputDTO>>
    {
        public DataContext _db;
     
        public GetAllOrdersHandlers(DataContext db)
        {
            _db = db;
           

        }
       

        public async Task<List<OrderSummaryOutputDTO>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", query._token.Split(" ")[1]);
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                var responsestring = await response.Content.ReadAsStringAsync();
                var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                    new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var productdetails = productlist.Select(e => e.Id).ToList();
                var productarr = await _db.OrderProducts.ToListAsync();



                var orderarr = await _db.Orders.ToListAsync();

                var groupjoin = (from o in orderarr.AsQueryable()
                                  join p in productarr.AsQueryable() on o.Id equals p.OrderId into g
                                  select new OrderSummaryOutputDTO
                                  {
                                      Id = o.Id,
                                     email = o.email,
                                      Status_Of_Request = o.Status_Of_Request,
                                      CreatedTime = o.CreatedTime,
                                      AcceptedTime  = o.AcceptedTime,
                                      orderProducts = g.ToList(),
                                  }


                           ).ToList();




                return groupjoin;
                //foreach (var productId in command.OrderInputDTO.ProductIds)
                //{
                //    var orderProduct = new OrderProduct
                //    {
                //        OrderId = order.Id,
                //        ProductId = productId
                //    };

                //    _db.OrderProducts.Add(orderProduct);

                //}

                //await _db.SaveChangesAsync();

                //var output = new OrderOutputDTO()
                //{
                //    Id = order.Id,
                //    UserId = order.Id,
                //    ProductId = command.OrderInputDTO.ProductIds,
                //    Status_Of_Request = order.Status_Of_Request,
                //    CreatedTime = DateTime.Now,
                //    AcceptedTime = DateTime.Now


                //};
                //return output;
            }




        }

    }
}
